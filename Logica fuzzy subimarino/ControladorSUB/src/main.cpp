#include <WiFi.h>
#include <WebServer.h>


// Configurações da rede Wi-Fi
const char* ssid = "Sant";  // Substitua pelo nome da sua rede
const char* password = "diniz123";  // Substitua pela senha da sua rede

// Cria um objeto servidor na porta 80
WebServer server(80);

// Página HTML com alavanca e estilo de submarino
const char* htmlPage = R"rawliteral(
  <!DOCTYPE html>
  <html>
  <head>
    <title>Controle do Reator - Submarino</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
      body {
        font-family: 'Courier New', Courier, monospace;
        background: linear-gradient(180deg, #1a2a3a, #0f1a27);
        color: #a0b0c0;
        text-align: center;
        margin: 0;
        padding: 20px;
        min-height: 100vh;
        overflow: hidden;
      }
      .submarine-container {
        background: rgba(50, 70, 90, 0.8);
        border: 2px solid #4a5a6a;
        border-radius: 10px;
        padding: 20px;
        max-width: 400px;
        margin: 0 auto;
        box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.5), 0 0 20px rgba(0, 255, 255, 0.2);
      }
      h1 {
        font-size: 24px;
        color: #00ccff;
        text-shadow: 0 0 5px #00ccff;
        margin-bottom: 20px;
      }
      .lever-container {
        position: relative;
        width: 100px;
        height: 300px;
        margin: 20px auto;
        background: linear-gradient(90deg, #3a4a5a, #2a3a4a);
        border: 2px solid #5a6a7a;
        border-radius: 10px;
        box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.7);
      }
      .lever {
        position: absolute;
        width: 80px;
        height: 40px;
        background: linear-gradient(90deg, #b22222, #8b0000);
        border: 2px solid #ffd700;
        border-radius: 5px;
        left: 8px;
        cursor: pointer;
        transition: transform 0.2s ease;
        box-shadow: 0 0 10px rgba(255, 0, 0, 0.5);
      }
      .lever:hover {
        background: linear-gradient(90deg, #d22222, #ab0000);
      }
      .labels {
        position: absolute;
        left: 120px;
        top: 0;
        height: 300px;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        color: #00ccff;
        font-size: 14px;
        text-shadow: 0 0 3px #00ccff;
      }
      #powerValue {
        font-size: 20px;
        color: #ffd700;
        text-shadow: 0 0 5px #ffd700;
        margin-top: 10px;
      }
      .sonar-ping {
        position: fixed;
        bottom: 20px;
        right: 20px;
        width: 10px;
        height: 10px;
        background: #00ff00;
        border-radius: 50%;
        box-shadow: 0 0 10px #00ff00;
        animation: ping 2s infinite;
      }
      @keyframes ping {
        0% { transform: scale(1); opacity: 1; }
        100% { transform: scale(3); opacity: 0; }
      }
    </style>
  </head>
  <body>
    <div class="submarine-container">
      <h1>Controle do Reator</h1>
      <div class="lever-container">
        <div class="lever" id="lever"></div>
        <div class="labels">
          <span>3500</span>
          <span>2625</span>
          <span>1750</span>
          <span>875</span>
          <span>0</span>
        </div>
      </div>
      <p>Potência: <span id="powerValue">0</span></p>
    </div>
    <div class="sonar-ping"></div>
  
    <script>
      const lever = document.getElementById('lever');
      let isDragging = false;
      let currentValue = 0;
  
      function updateLeverPosition(value) {
        const maxHeight = 260; // Altura interna do container (300px - 40px da alavanca)
        const position = maxHeight * (1 - value / 3500);
        lever.style.transform = `translateY(${position}px)`;
        document.getElementById('powerValue').innerText = value;
        fetch('/setPower?value=' + value)
          .then(response => response.text())
          .then(data => console.log(data))
          .catch(error => console.error('Erro:', error));
      }
  
      lever.addEventListener('mousedown', () => { isDragging = true; });
      lever.addEventListener('touchstart', () => { isDragging = true; });
  
      document.addEventListener('mousemove', (e) => {
        if (isDragging) {
          const rect = document.querySelector('.lever-container').getBoundingClientRect();
          let y = e.clientY - rect.top - 20; // Ajusta para o centro da alavanca
          y = Math.max(0, Math.min(y, 260)); // Limita ao container
          currentValue = Math.round(3500 * (1 - y / 260));
          updateLeverPosition(currentValue);
        }
      });
  
      document.addEventListener('touchmove', (e) => {
        if (isDragging) {
          const rect = document.querySelector('.lever-container').getBoundingClientRect();
          let y = e.touches[0].clientY - rect.top - 20;
          y = Math.max(0, Math.min(y, 260));
          currentValue = Math.round(3500 * (1 - y / 260));
          updateLeverPosition(currentValue);
        }
      });
  
      document.addEventListener('mouseup', () => { isDragging = false; });
      document.addEventListener('touchend', () => { isDragging = false; });
  
      // Inicializa a alavanca na posição 0
      updateLeverPosition(0);
    </script>
  </body>
  </html>
  )rawliteral";

// Declaração das funções
void handleRoot();
void handleSetPower();

void setup() {
  // Inicia a comunicação serial
  Serial.begin(9600);

  // Conecta à rede Wi-Fi
  WiFi.begin(ssid, password);
  delay(5000);
  Serial.print(WiFi.localIP());

  // Configura as rotas do servidor
  server.on("/", handleRoot); // Página inicial
  server.on("/setPower", handleSetPower); // Rota para configurar a potência

  // Inicia o servidor
  server.begin();

}

void loop() {
  // Lida com requisições do cliente
  server.handleClient();
}

// Função para servir a página HTML
void handleRoot() {
  server.send(200, "text/html", htmlPage);
}

// Função para lidar com a configuração da potência
void handleSetPower() {
  if (server.hasArg("value")) {
    String powerValue = server.arg("value");
    int power = powerValue.toInt();
    Serial.println(power); // Envia o valor pela serial
    server.send(200, "text/plain", "Potência configurada: " + powerValue);
  } else {
    server.send(400, "text/plain", "Parâmetro 'value' não encontrado");
  }
}