using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Neo.SmartContract
{
    public class GetBalance_Template : Framework.SmartContract
    {
        delegate object deleDyncall(string method, object[] arr);

        public static Object Main(string operation, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Application)
            {
                if (operation == "getBalance") return BalancesOf(args[0] as byte[], args);
            }
            return false;
        }

        private static BigInteger[] BalancesOf(byte[] asset, params object[] args)
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
    }
}
