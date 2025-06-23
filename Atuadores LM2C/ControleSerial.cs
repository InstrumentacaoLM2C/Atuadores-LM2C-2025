using System;
using System.IO;
using System.IO.Ports;

namespace Atuadores_LM2C
{
    public class ControleSerial : IDisposable
    {
        private SerialPort serialPort;
        private readonly object lockObj = new object(); // Evita concorrência em leitura/escrita

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
            serialPort.ReadTimeout = 1000; // 1000 ms
            serialPort.WriteTimeout = 1000; // 1000 ms

            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                throw new IOException($"Erro ao abrir a porta serial: {ex.Message}", ex);
            }
        }

        public void Desconectar()
        {
            try
            {
                if (serialPort.IsOpen)
                    serialPort.Close();

                serialPort.DataReceived -= SerialPort_DataReceived; // Desanexar evento
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao desconectar a porta serial: " + ex.Message);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (lockObj)
                {
                    string dados = serialPort.ReadExisting().Trim();
                    var handler = DadosRecebidos;
                    if (handler != null)
                    {
                        handler(this, dados);
                    }
                }
            }
            catch (Exception ex)
            {
                var handler = DadosRecebidos;
                if (handler != null)
                {
                    handler(this, $"[Erro na leitura serial: {ex.Message}]");
                }
            }
        }

        public void Enviar(string mensagem)
        {
            if (mensagem == null) return;

            try
            {
                lock (lockObj)
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
            }
            catch (Exception ex)
            {
                throw new IOException($"Erro ao enviar dados pela serial: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            if (serialPort != null)
            {
                try
                {
                    if (serialPort.IsOpen)
                        serialPort.Close();
                }
                catch { }

                serialPort.DataReceived -= SerialPort_DataReceived;
                serialPort.Dispose();
                serialPort = null;
            }
        }
    }
}
