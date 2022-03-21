﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Timetable.Redis
{
    public class RedisOperater : IDisposable
    {
        private ConnectionMultiplexer redis { get; set; }
        private IDatabase db { get; set; }
        public RedisOperater(string connection)
        {
            redis = ConnectionMultiplexer.Connect(connection);
            db = redis.GetDatabase();
        }
        #region string类型操作
        /// <summary>
        /// set or update the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, string value)
        {
            return db.StringSet(key, value);
        }
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool SetValue(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return db.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SetValue<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {

            string json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key) where T : class
        {
            var result = db.StringGet(key);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception)
            {
            }
            return null;
        }
        /// <summary>
        /// get the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// Delete the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteValue(string key)
        {
            return db.KeyDelete(key);
        }
        #endregion

        #region 哈希类型操作
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetHashValue(string key, string hashkey, string value)
        {
            return db.HashSet(key, hashkey, value);
        }
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="t">defined class</param>
        /// <returns></returns>
        public bool SetHashValue<T>(String key, string hashkey, T t) where T : class
        {
            var json = JsonConvert.SerializeObject(t);
            return db.HashSet(key, hashkey, json);
        }
        /// <summary>
        /// 保存一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="list">数据集合</param>
        /// <param name="getModelId"></param>
        public void HashSet<T>(string key, List<T> list, Func<T, string> getModelId)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonConvert.SerializeObject(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            db.HashSet(key, listHashEntry.ToArray());
        }
        /// <summary>
        /// 获取hashkey所有的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> HashGetAll<T>(string key) where T : class
        {
            List<T> result = new List<T>();
            HashEntry[] arr = db.HashGetAll(key);
            foreach (var item in arr)
            {
                if (!item.Value.IsNullOrEmpty)
                {
                    try
                    {
                        var t = JsonConvert.DeserializeObject<T>(item.Value);
                        result.Add(t);
                    }
                    catch (Exception)
                    {

                    }
                }

            }
            return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public RedisValue GetHashValue(string key, string hashkey)
        {
            RedisValue result = db.HashGet(key, hashkey);
            return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public T GetHashValue<T>(string key, string hashkey) where T : class
        {
            RedisValue result = db.HashGet(key, hashkey);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception)
            {
            }
            return null;
        }
        /// <summary>
        /// delete the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public bool DeleteHashValue(string key, string hashkey)
        {

            return db.HashDelete(key, hashkey);
        }

        public void Dispose()
        {
            try
            {
                redis.Dispose();
            }
            catch (Exception)
            {


            }
        }
        #endregion
    }

}
