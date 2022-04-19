String.format = function() {
	var s = arguments[0];
	for (var i = 0; i < arguments.length - 1; i++) {
		var reg = new RegExp("\\{" + i + "\\}", "gm");
		s = s.replace(reg, arguments[i + 1]);
	}
	return s;
}

import GPS from './GPS.js'

function MapAPI(key, options) {
	options = options;

	/**
	 * 地址反查
	 * @param {Object} point 坐标点
	 * @param {Function} callback 回调函数
	 */
	this.getPoi = function(point, callback) {
		var url = String.format(options.poiurl, String.format(options.locationRaw, point.latitude, point.longitude), key);
		uni.request({
			url: url,
			success: (res) => {
				if (res.data.regeocode) {
					res.data.result = res.data.regeocode
				}
				if (res.data.result && res.data.result.pois) {

					var pois = res.data.result.pois
					if (pois.length > 0 && !pois[0].title && pois[0].name) {
						for (var i = 0; i < pois.length; i++) {
							pois[i].title = pois[i].name;
							
						}
					}

					if (options.mapName == "aMap" || options.mapName == "baiduMap") {
						res.data.result.pois = pois.sort((a, b) => {
							return a.distance - b.distance
						})
					}

					if (options.mapName == "baiduMap") {
						for (var i = 0; i < pois.length; i++) {
							pois[i].address=pois[i].addr
							pois[i].id=pois[i].uid;
							pois[i].location = {
								lat: pois[i].point.y,
								lng: pois[i].point.x
							}
						}
					}
				}
				callback(res);
			}
		})
	}




	/**
	 * 搜索地点
	 * @param {String} keyword 搜索关键词
	 * @param {String} region  区域
	 * @param {Function} callback 回调函数
	 */
	this.getSearchList = function(keyword, region, callback) {
		var url = String.format(options.searchUrl, keyword, region, key);
		console.log(url)
		uni.request({
			url: url,
			success: (res) => {
				if (options.mapName == "aMap") {
					res.data.data = res.data.pois;
					res.data.data.forEach(item => {
						var sp = item.location.split(',');
						item.location = {
							lat: sp[1],
							lng: sp[0]
						}
						item.title = item.name
					})
				}
				if(options.mapName=='baiduMap'){
					res.data.data=res.data.results;
					res.data.data.forEach(item => {
						item.id=item.uid;
						item.title = item.name
					})
				}
				callback(res);
			}
		})
	}


	/**
	 * 距离计算
	 * @param {Object} fromPoint 出发点（1）
	 * @param {Object} toPoints  目标点（多）
	 * @param {Function} callback 回调函数
	 */
	this.getDistance = function(fromPoint, toPoints, callback) {
		if (options.mapName == "baiduMap") {
			callback(baiduMapGetDistance(fromPoint,toPoints))
		} else {
			var fromStr = String.format(options.locationRaw, fromPoint.latitude, fromPoint.longitude);
			var toArr = [];
			var toStr;
			toArr = toPoints.map((item) => {
				if (typeof(item.location) == "object") {
					return String.format(options.locationRaw, item.location.lat, item.location.lng)
				} else {
					return item.location
				}
			})

			console.log(toArr)
			if (options.mapName == "aMap") {
				toStr = toArr.join('|');
			} else {
				toStr = toArr.join(';');
			}
			var url = String.format(options.distanceUrl, fromStr, toStr, key);

			uni.request({
				url: url,
				success: (res) => {
					if (options.mapName == "aMap") {
						res.data.result = {
							elements: res.data.results
						};
					}
					callback(res);
				}
			})
		}
	}

	function baiduMapGetDistance(fromPoint, toPoints) {
		fromPoint={
			lat:fromPoint.latitude,
			lng:fromPoint.longitude
		}
		var res = {
			data: {
				result: {
					elements: []
				}
			},
		};
		var elements = res.data.result.elements;
		
		toPoints.forEach(item => {
			
			elements.push({distance:GPS.distance(fromPoint, item.location).toFixed(0)});
		})
		return res;
	}
}


const mapKey = {
	tencentMap: 'CB7BZ-RPJWF-GVMJD-JOQYN-RG7EH-ALBT4',
	aMap: '63b323114b7664144354a92578c02b39',
	baiduMap: 'GdLKeME8hdEQAbDmtMeEhccgtXNXkQgl'
};

var tencentMapApi = new MapAPI(mapKey.tencentMap, {
	locationRaw: '{0},{1}',
	mapName: 'tencentMap',
	searchUrl: `https://apis.map.qq.com/ws/place/v1/search?keyword={0}&boundary=region({1},0)&orderby=_distance&page_size=20&key={2}`,
	"distanceUrl": 'https://apis.map.qq.com/ws/distance/v1/?mode=walking&from={0}&to={1}&key={2}',
	"poiurl": `https://apis.map.qq.com/ws/geocoder/v1/?get_poi=1&location={0}&poi_options=address_format=short;radius=400;page_size=20;page_index=1;policy=2&key={1}`,
})

var aMapApi = new MapAPI(mapKey.aMap, {
	locationRaw: '{1},{0}',
	mapName: 'aMap',
	searchUrl: `https://restapi.amap.com/v3/place/text?keywords={0}&city={1}&citylimit=1&key={2}`,
	"distanceUrl": 'https://restapi.amap.com/v3/distance?destination={0}&origins={1}&type=0&key={2}',
	"poiurl": `https://restapi.amap.com/v3/geocode/regeo?homeorcorp=0&location={0}&radius=400&extensions=all&key={1}`,
})

var baiduMapApi = new MapAPI(mapKey.baiduMap, {
	mapName: 'baiduMap',
	locationRaw: '{0},{1}',
	searchUrl:'http://api.map.baidu.com/place/v2/search?query={0}&city_limit=true&region={1}&output=json&ak={2}&coord_type=2&ret_coordtype=gcj02ll&page_size=20',
	distanceUrl: 'https://api.map.baidu.com/routematrix/v2/walking?origins={0}&destinations={1}&coord_type=gcj02&ak={2}',
	poiurl: 'https://api.map.baidu.com/reverse_geocoding/v3/?coordtype=gcj02ll&ret_coordtype=gcj02ll&location={0}&radius=1000&extensions_poi=1&output=json&ak={1}'
})


export {
	tencentMapApi,
	aMapApi,
	baiduMapApi
};
