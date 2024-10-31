using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        private const double TAXA = 3.5;
        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double DepositoInicial { get; private set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular)
        {
            this.Numero = numero;
            this.Titular = titular;
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            this.Numero = numero;
            this.Titular = titular;
            Deposito(depositoInicial);
        }

        public void Deposito(double quantia)
        {
            if (quantia > 0)
                this.Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            if (quantia > 0)
                this.Saldo -= (quantia + TAXA);
        }
        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}