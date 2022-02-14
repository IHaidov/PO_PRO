﻿using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StackExchange.Redis;
namespace PO_PRO
{
    public static class DB
    {
        public static bool Redis_initialized;


        private static readonly ConfigurationOptions configurationOptions = new ConfigurationOptions
        {
            EndPoints = { { "redis-16263.c16.us-east-1-2.ec2.cloud.redislabs.com", 16263 } },
            Ssl = false,
            AsyncTimeout = 60000,
            SyncTimeout = 60000,
            //AllowAdmin = true,
            //SslProtocols = SslProtocols.Auto,
            DefaultDatabase = 0,
            Password = "ElNbPom1CbKYMeYRxPKFswAl2DxbWDz9",
            User = "default",
            AbortOnConnectFail = true,
            ReconnectRetryPolicy = new LinearRetry(60000),
            ConnectRetry = 3,
            ConnectTimeout = 60000,
        };




        public static IDatabase db;
        private static ConnectionMultiplexer redis;

        public static bool Try_connect(out Exception ex)
        {
            if (redis?.IsConnected == true)
            {
                ex = null;
                return true;
            }

            try
            {
                redis = ConnectionMultiplexer.Connect(configurationOptions);

                db = redis.GetDatabase(0);

                Console.WriteLine("Successfully connected to the DB");

                ex = null;

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                ex = e;
                return false;
            }
        }

        public static bool WriteHash(string key, HashEntry[] value)
        {
            while (true)
                try
                {
                    if (!Try_connect(out var exception))
                    {
                        Console.Write($"RedisDB 'Write' attempt failed. EX : {exception}. Sleeping 5s and retrying");
                        continue;
                    }

                    db.HashSet(key, value);

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Write exception : {e}");
                }
        }


        public static bool Write(string key, string value)
        {
            while (true)
                try
                {
                    if (!Try_connect(out var exception))
                    {
                        Console.Write($"RedisDB 'Write' attempt failed. EX : {exception}. Sleeping 5s and retrying");
                        continue;
                    }

                    db.StringSet(key, value);

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Write exception : {e}");
                }
        }

        public static bool Delete_key(string key)
        {
            while (true)
                try
                {
                    if (!Try_connect(out var exception))
                    {
                        Console.Write($"RedisDB 'Write' attempt failed. EX : {exception}. Sleeping 5s and retrying");
                        continue;
                    }

                    db.KeyDelete(key);

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Write exception : {e}");
                }
        }

        public static bool Delete_key_Hash(string key, string name)
        {
            while (true)
                try
                {
                    if (!Try_connect(out var exception))
                    {
                        Console.Write($"RedisDB 'Write' attempt failed. EX : {exception}. Sleeping 5s and retrying");
                        continue;
                    }

                    db.HashDelete(key, name);

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Write exception : {e}");
                }
        }


        public static bool Key_exist(string key)
        {
            while (true)
                try
                {
                    if (Redis_initialized) return db.KeyExists(key);

                    var redis = ConnectionMultiplexer.Connect(configurationOptions);

                    db = redis.GetDatabase(0);

                    Redis_initialized = true;

                    return db.KeyExists(key);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Check key exception : {e}");
                }
        }

        public static bool Read(string key, out byte[] answer)
        {
            while (true)
                try
                {
                    if (!Redis_initialized)
                    {
                        var redis = ConnectionMultiplexer.Connect(configurationOptions);
                        db = redis.GetDatabase(0);
                        Redis_initialized = true;
                    }

                    answer = db.StringGet(key);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Read exception : {e}");
                }
        }

        public static bool Read(string key, out string answer)
        {
            while (true)
                try
                {
                    if (!Redis_initialized)
                    {
                        var redis = ConnectionMultiplexer.Connect(configurationOptions);
                        db = redis.GetDatabase(0);
                        Redis_initialized = true;
                    }

                    answer = db.StringGet(key);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Read exception : {e}");
                }
        }

        public static bool Write(string key, string value, TimeSpan? expiration = null)
        {
            while (true)
                try
                {
                    if (!Try_connect(out var exception))
                    {
                        Console.Write($"RedisDB 'Write' attempt failed. EX : {exception}. Sleeping 5s and retrying");
                        continue;
                    }

                    db.StringSet(key, value, expiration);

                    //db.StringSet(key, value, expiration);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Write exception : {e}");
                }
        }

        public static bool Write(string key, byte[] value, TimeSpan expires = default)
        {
            while (true)
                try
                {
                    if (!Try_connect(out var exception))
                    {
                        Console.Write($"RedisDB 'Write' attempt failed. EX : {exception}. Sleeping 5s and retrying");
                        continue;
                    }
                    db.StringSet(key, value, expires);

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Write exception : {e}");
                }
        }


        public static bool Loop_untill_success<TSource>(Func<TSource> func, out TSource result)
        {

            while (true)
                try
                {
                    if (!Redis_initialized)
                    {
                        var redis = ConnectionMultiplexer.Connect(configurationOptions);
                        db = redis.GetDatabase(0);
                        Redis_initialized = true;
                    }

                    result = func.Invoke();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Function {func.Method.Name} call exception  : {e}");
                }
        }

        public static bool TryRead(string key, out string answer, out Exception exception)
        {
            try
            {
                if (!Redis_initialized)
                {
                    var redis = ConnectionMultiplexer.Connect(configurationOptions);
                    db = redis.GetDatabase(0);
                    Redis_initialized = true;
                }

                answer = db.StringGet(key);
                exception = null;
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                answer = null;
                return false;
            }
        }

        public static List<string> GetAllkeys()
        {
            Redis_initialized = false;
           
            List<string> listKeys = new List<string>();
            try
            {
                
                
               
                var redis = ConnectionMultiplexer.Connect(configurationOptions);
                   
               // db = redis.GetDatabase(0);
                   
                  
                
                var keys = redis.GetServer("redis-16263.c16.us-east-1-2.ec2.cloud.redislabs.com", 16263).Keys();
                

                listKeys.AddRange(keys.Select(key => (string)key).ToList());

            }
            catch (Exception e)
            {
                MessageBox.Show($" exception  : {e}");
            }
            return listKeys;
        }
        public static bool ReadAll(out Dictionary<string, string> answer)
        {
            
            List<string> keys = GetAllkeys();
            foreach (var VARIABLE in keys)
            {
                Console.WriteLine(VARIABLE.ToString());
            }
            answer = new Dictionary<string, string>();
            while (true)
                try
                {
                    if (!Redis_initialized)
                    {
                        var redis = ConnectionMultiplexer.Connect(configurationOptions);
                        db = redis.GetDatabase(0);
                        Redis_initialized = true;
                    }

                    foreach (var key in keys)
                    {
                        answer.Add(key, db.StringGet(key));
                    }
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Read exception : {e}");
                }
        }
    }
}
