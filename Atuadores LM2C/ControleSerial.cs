using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace Atuadores_LM2C
{
    public class ControleSerial : IDisposable
    {
        private SerialPort serialPort;

        public event EventHandler<string> DadosRecebidos;

        public bool EstaConectado => serialPort != null && serialPort.IsOpen;

        public ControleSerial()
        {
            serialPort = new SerialPort();
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Conectar(string portName, int baudRate = 9600)
        {
            if (serialPort.IsOpen)
                serialPort.Close();

            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.DtrEnable = true;

            serialPort.Open();
        }

        public void Desconectar()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string dados = serialPort.ReadLine();
                DadosRecebidos?.Invoke(this, dados);
            }
            catch (Exception ex)
            {
                DadosRecebidos?.Invoke(this, $"[Erro na leitura serial: {ex.Message}]");
            }
        }

        public void Enviar(string mensagem)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.WriteLine(mensagem);
            }
            else
            {
                throw new InvalidOperationException("Porta serial não está conectada.");
            }
        }

        public void Dispose()
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                    serialPort.Close();

                serialPort.Dispose();
                serialPort = null;
            }
        }


    }


}
