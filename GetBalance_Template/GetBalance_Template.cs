using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using Neo.SmartContract.Framework.Services.System;
using System;
using System.Numerics;

namespace Neo.SmartContract
{
    public class GetBalance_Template : Framework.SmartContract
    {
        delegate object deleDyncall(string method, object[] arr);

        //args全部参数使用UInt160类型传入
        public static Object Main(string operation, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Application)
            {
                if (operation == "getBalanceOf") return GetBalanceOf(args[0] as byte[], args);
                if (operation == "balanceOf") return BalanceOf(args[0] as byte[], args[1] as byte[]);
            }
            return false;
        }
        //获取一组地址的余额, args[0]为资产id, args[1]之后为地址集
        private static BigInteger[] GetBalanceOf(byte[] asset, params object[] args)
        {
            deleDyncall dyncall = (deleDyncall)asset.ToDelegate();
            BigInteger[] balances = new BigInteger[args.Length - 1];
            for (int i = 0; i < args.Length - 1; i++)
            {
                object[] new_args = new object[1];
                new_args[0] = args[i + 1];
                balances[i] = (BigInteger)dyncall("balanceOf", new_args);
            }
            return balances;
        }
        //BalanceOf调用BalanceOf
        private static BigInteger BalanceOf(byte[] asset, byte[] address)
        {
            object[] new_args = new object[1];
            new_args[0] = address;

            deleDyncall dyncall = (deleDyncall)asset.ToDelegate();
            return (BigInteger)dyncall("balanceOf", new_args);
        }
    }
}
