import numpy as np
import skfuzzy as fuzz
import matplotlib.pyplot as plt
import serial
import time
try:
    porta_recebimento = serial.Serial("COM12", 9600, timeout=1)
    time.sleep(2)
    print("Porta aberta com sucesso!")
except Exception as e:
    print("Erro ao abrir porta:", e)

# Gerar variáveis ​​do universo
# * Qualidade e serviço em intervalos subjetivos [0, 10]
# * Dica tem um intervalo de [0, 25] em unidades de pontos percentuais
X_DiferençaReator = np.arange(-3500, 3500, 1)
X_PReator = np.arange(-6, 6, 0.01)
X_DiferençaTurbina = np.arange(-3500, 3500, 1)
X_PTurbina = np.arange(-10, 10, 0.01)
X_Temperatura = np.arange(0, 100, 0.01)

# Gerar funções de associação fuzzy
ReatorGO = fuzz.trapmf(X_DiferençaReator, [-3501, -3501, -1500, -1000])
ReatorMO = fuzz.trimf(X_DiferençaReator, [-1500, -790, -80])
ReatorPO = fuzz.trimf(X_DiferençaReator, [-500, 0, 0])
ReatorPI = fuzz.trimf(X_DiferençaReator, [0, 0, 500])
ReatorMI = fuzz.trimf(X_DiferençaReator, [80, 790, 1500])
ReatorGI = fuzz.trapmf(X_DiferençaReator, [1000, 1500, 3501, 3501])

PReatorGO = fuzz.trapmf(X_PReator, [-6,-6, -5.4, -1.4])
PReatorMO = fuzz.trimf(X_PReator, [-2.5, -1.3, -0.09])
PReatorPO = fuzz.trimf(X_PReator, [-0.1, 0, 0.06])
PReatorPI = fuzz.trimf(X_PReator, [-0.06, 0, 0.1])
PReatorMI = fuzz.trimf(X_PReator, [0.09, 1.3, 2.5])
PReatorGI = fuzz.trapmf(X_PReator, [1.4, 5.4, 6,6])

TurbinaGO = fuzz.trapmf(X_DiferençaTurbina, [-3501, -3501, -1500, -1000])
TurbinaMO = fuzz.trimf(X_DiferençaTurbina, [-1500, -790, -80])
TurbinaPO = fuzz.trimf(X_DiferençaTurbina, [-500, 0, 0])
TurbinaPI = fuzz.trimf(X_DiferençaTurbina, [0, 0, 500])
TurbinaMI = fuzz.trimf(X_DiferençaTurbina, [80, 790, 1500])
TurbinaGI = fuzz.trapmf(X_DiferençaTurbina, [1000, 1500, 3501, 3501])

PTurbinaGO = fuzz.trapmf(X_PTurbina, [-10,-10, -5.4, -1.4])
PTurbinaMO = fuzz.trimf(X_PTurbina, [-2.5, -1.3, -0.09])
PTurbinaPO = fuzz.trimf(X_PTurbina, [-0.1, 0, 0.06])
PTurbinaPI = fuzz.trimf(X_PTurbina, [-0.06, 0, 0.1])
PTurbinaMI = fuzz.trimf(X_PTurbina, [0.09, 1.3, 2.5])
PTurbinaGI = fuzz.trapmf(X_PTurbina, [1.4, 5.4, 10,10])

Estavel = fuzz.trimf(X_Temperatura, [-0.1, 0, 70])
superaquecendo = fuzz.trimf(X_Temperatura, [60, 100, 100])

try:
    while True:
        if porta_recebimento.in_waiting > 0:
            dados = porta_recebimento.readline().decode('utf-8').strip()
            Valores = dados.strip().split(";") 
            
            X_Reator = float(Valores[0].replace(",", "."))
            X_Turbina = float(Valores[1].replace(",", "."))
            X_Temp = float(Valores[2].replace(",", "."))

            if X_Temp <= 0:
                X_Temp = 0
            elif X_Temp >= 100:
                X_Temp = 99

            if  X_Reator <= -3500:
                X_Reator = -3500
            elif X_Reator >= 3500:
                X_Reator = 3500
            
            if  X_Turbina <= -3500:
                X_Turbina = -3500
            elif X_Turbina >= 3500:
                X_Turbina = 3500

            ReatorlevelPO = fuzz.interp_membership(X_DiferençaReator, ReatorPO , X_Reator)
            ReatorlevelMO = fuzz.interp_membership(X_DiferençaReator, ReatorMO, X_Reator)
            ReatorlevelGO = fuzz.interp_membership(X_DiferençaReator, ReatorGO, X_Reator)
            ReatorlevelPI = fuzz.interp_membership(X_DiferençaReator, ReatorPI, X_Reator)
            ReatorlevelMI = fuzz.interp_membership(X_DiferençaReator, ReatorMI, X_Reator)
            ReatorlevelGI = fuzz.interp_membership(X_DiferençaReator, ReatorGI, X_Reator)

            TurbinalevelPO = fuzz.interp_membership(X_DiferençaTurbina, TurbinaPO, X_Turbina)
            TurbinalevelMO = fuzz.interp_membership(X_DiferençaTurbina, TurbinaMO, X_Turbina)
            TurbinalevelGO = fuzz.interp_membership(X_DiferençaTurbina, TurbinaGO, X_Turbina)
            TurbinalevelPI = fuzz.interp_membership(X_DiferençaTurbina, TurbinaPI, X_Turbina)
            TurbinalevelMI = fuzz.interp_membership(X_DiferençaTurbina, TurbinaMI, X_Turbina)
            TurbinalevelGI = fuzz.interp_membership(X_DiferençaTurbina, TurbinaGI, X_Turbina)

            TemplevelEstavel = fuzz.interp_membership(X_Temperatura, Estavel, X_Temp)
            Templevelsuperaquecido = fuzz.interp_membership(X_Temperatura, superaquecendo, X_Temp)


            ReatorlevelPO = fuzz.interp_membership(X_DiferençaReator, ReatorPO , X_Reator)
            ReatorlevelMO = fuzz.interp_membership(X_DiferençaReator, ReatorMO, X_Reator)
            ReatorlevelGO = fuzz.interp_membership(X_DiferençaReator, ReatorGO, X_Reator)
            ReatorlevelPI = fuzz.interp_membership(X_DiferençaReator, ReatorPI, X_Reator)
            ReatorlevelMI = fuzz.interp_membership(X_DiferençaReator, ReatorMI, X_Reator)
            ReatorlevelGI = fuzz.interp_membership(X_DiferençaReator, ReatorGI, X_Reator)

            TurbinalevelPO = fuzz.interp_membership(X_DiferençaTurbina, TurbinaPO, X_Turbina)
            TurbinalevelMO = fuzz.interp_membership(X_DiferençaTurbina, TurbinaMO, X_Turbina)
            TurbinalevelGO = fuzz.interp_membership(X_DiferençaTurbina, TurbinaGO, X_Turbina)
            TurbinalevelPI = fuzz.interp_membership(X_DiferençaTurbina, TurbinaPI, X_Turbina)
            TurbinalevelMI = fuzz.interp_membership(X_DiferençaTurbina, TurbinaMI, X_Turbina)
            TurbinalevelGI = fuzz.interp_membership(X_DiferençaTurbina, TurbinaGI, X_Turbina)

            TemplevelEstavel = fuzz.interp_membership(X_Temperatura, Estavel, X_Temp)
            Templevelsuperaquecido = fuzz.interp_membership(X_Temperatura, superaquecendo, X_Temp)

            ReatorRegra1 = np.fmin(ReatorlevelPI,TemplevelEstavel)
            ReatorRegra2 = np.fmin(ReatorlevelMI,TemplevelEstavel)
            ReatorRegra3 = np.fmin(ReatorlevelGI,TemplevelEstavel)
            ReatorRegra4 = np.fmin(ReatorlevelPI,Templevelsuperaquecido)
            ReatorRegra5 = np.fmin(ReatorlevelMI,Templevelsuperaquecido)
            ReatorRegra6 = np.fmin(ReatorlevelGI,Templevelsuperaquecido)
            ReatorRegra7 = np.fmin(ReatorlevelPO,TemplevelEstavel)
            ReatorRegra8 = np.fmin(ReatorlevelMO,TemplevelEstavel)
            ReatorRegra9 = np.fmin(ReatorlevelGO,TemplevelEstavel)
            ReatorRegra10 = np.fmin(ReatorlevelPO,Templevelsuperaquecido)
            ReatorRegra11 = np.fmin(ReatorlevelMO,Templevelsuperaquecido)
            ReatorRegra12 = np.fmin(ReatorlevelGO,Templevelsuperaquecido)

            Reatorativasao1 = np.fmin(ReatorRegra1,PReatorPO)
            Reatorativasao2 = np.fmin(ReatorRegra2,PReatorMO)
            Reatorativasao3 = np.fmin(ReatorRegra3,PReatorGO)
            Reatorativasao4 = np.fmin(ReatorRegra4,PReatorGO)
            Reatorativasao5 = np.fmin(ReatorRegra5,PReatorGO)
            Reatorativasao6 = np.fmin(ReatorRegra6,PReatorGO)
            Reatorativasao7 = np.fmin(ReatorRegra7,PReatorPI)
            Reatorativasao8 = np.fmin(ReatorRegra8,PReatorMI)
            Reatorativasao9 = np.fmin(ReatorRegra9,PReatorGI)
            Reatorativasao10 = np.fmin(ReatorRegra10,PReatorGO)
            Reatorativasao11 = np.fmin(ReatorRegra11,PReatorGO)
            Reatorativasao12 = np.fmin(ReatorRegra12,PReatorGO)

            TurbinaRegra1 = np.fmin(TurbinalevelPI,TemplevelEstavel)
            TurbinaRegra2 = np.fmin(TurbinalevelMI,TemplevelEstavel)
            TurbinaRegra3 = np.fmin(TurbinalevelGI,TemplevelEstavel)
            TurbinaRegra4 = np.fmin(TurbinalevelPI,Templevelsuperaquecido)
            TurbinaRegra5 = np.fmin(TurbinalevelMI,Templevelsuperaquecido)
            TurbinaRegra6 = np.fmin(TurbinalevelGI,Templevelsuperaquecido)
            TurbinaRegra7 = np.fmin(TurbinalevelPO,TemplevelEstavel)
            TurbinaRegra8 = np.fmin(TurbinalevelMO,TemplevelEstavel)
            TurbinaRegra9 = np.fmin(TurbinalevelGO,TemplevelEstavel)
            TurbinaRegra10 = np.fmin(TurbinalevelPO,Templevelsuperaquecido)
            TurbinaRegra11 = np.fmin(TurbinalevelMO,Templevelsuperaquecido)
            TurbinaRegra12 = np.fmin(TurbinalevelGO,Templevelsuperaquecido)

            Turbinaativasao1 = np.fmin(TurbinaRegra1,PTurbinaPO)
            Turbinaativasao2 = np.fmin(TurbinaRegra2,PTurbinaMO)
            Turbinaativasao3 = np.fmin(TurbinaRegra3,PTurbinaGO)
            Turbinaativasao4 = np.fmin(TurbinaRegra4,PTurbinaGI)
            Turbinaativasao5 = np.fmin(TurbinaRegra5,PTurbinaGI)
            Turbinaativasao6 = np.fmin(TurbinaRegra6,PTurbinaGI)
            Turbinaativasao7 = np.fmin(TurbinaRegra7,PTurbinaPI)
            Turbinaativasao8 = np.fmin(TurbinaRegra8,PTurbinaMI)
            Turbinaativasao9 = np.fmin(TurbinaRegra9,PTurbinaGI)
            Turbinaativasao10 = np.fmin(TurbinaRegra10,PTurbinaGI)
            Turbinaativasao11 = np.fmin(TurbinaRegra11,PTurbinaGI)
            Turbinaativasao12 = np.fmin(TurbinaRegra12,PTurbinaGI)

            tip0 = np.zeros_like(X_PReator)
            tip1 = np.zeros_like(X_PTurbina)

            aggregated_reator = np.fmax(Reatorativasao1,
                np.fmax(Reatorativasao2,
                np.fmax(Reatorativasao3,
                np.fmax(Reatorativasao4,
                np.fmax(Reatorativasao5,
                np.fmax(Reatorativasao6,
                np.fmax(Reatorativasao7,
                np.fmax(Reatorativasao8,
                np.fmax(Reatorativasao9,
                np.fmax(Reatorativasao10,
                np.fmax(Reatorativasao11,
                        Reatorativasao12)))))))))))

            aggregated_turbina = np.fmax(Turbinaativasao1,
                np.fmax(Turbinaativasao2,
                np.fmax(Turbinaativasao3,
                np.fmax(Turbinaativasao4,
                np.fmax(Turbinaativasao5,
                np.fmax(Turbinaativasao6,
                np.fmax(Turbinaativasao7,
                np.fmax(Turbinaativasao8,
                np.fmax(Turbinaativasao9,
                np.fmax(Turbinaativasao10,
                np.fmax(Turbinaativasao11,
                        Turbinaativasao12)))))))))))

            tipreator1 = fuzz.defuzz(X_PReator, aggregated_reator, 'centroid')  
            tipturbina2 = fuzz.defuzz(X_PTurbina, aggregated_turbina, 'centroid')  

            mensagem = f"{tipreator1:.2f};{tipturbina2:.2f}\n"
            porta_recebimento.write(mensagem.encode())


finally:
    porta_recebimento.close()