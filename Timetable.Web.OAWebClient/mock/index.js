/**
 * 查找并返回所有控制器方法
 */

var  fs =require( "fs");
var join=  require( "path").join;
const array = [];  


const getFiles = (jsonPath) => {
  const jsonFiles = []; 
  const findJsonFile = (path) => {
    const files = fs.readdirSync(path);
    files.forEach((item, index) => {
      const fPath = join(path, item); 
      const stat = fs.statSync(fPath);
      if (stat.isDirectory() === true) findJsonFile(item);
      if (stat.isFile() === true) jsonFiles.push(item);
    });
  };
  findJsonFile(jsonPath);
  jsonFiles.forEach((item, index) => array.push(`./controller/${item}`));
};
getFiles("mock/controller");
const mocks = []; 
array.forEach(async (item, index) => {
  const obj = require(item).default;
  await mocks.push(...obj);
});

module.exports= mocks;
