#FUENTE DE REFERENCIA PARA EL USO DE RFID CON RASPBERRY PI PICO
#https://www.youtube.com/watch?v=bvn_o39uXac&t=218s&pp=ygUWcmZpZCByYXNwYmVycnkgcGkgcGljbw%3D%3D

#GITHUB CON LA LIBRERIA PARA UTILIZAR EL RC522
#https://github.com/danjperron/micropython-mfrc522/blob/master/mfrc522.py


from machine import Pin
# Importa la librería necesaria para utilizar el lector RFID RC522
from mfrc522 import MFRC522

import random

import utime


# ==================== CONFIGURACIÓN DEL RFID ====================

# Configura el lector RFID RC522 utilizando comunicación SPI
lector = MFRC522(spi_id=0, sck=2, miso=4, mosi=3, cs=1, rst=0)


# Diccionario que relaciona el UID de cada tarjeta RFID
# con el número del jugador correspondiente
JUGADORES = {
    117251158: 1,
    313528259: 2,
    229049923: 3,
    119670426: 4,
}


# ==================== CONFIGURACIÓN DE LOS DISPLAYS ====================

#             a
#        ───────────
#       |           |
#    f  |           |  b
#       |           |
#       |           |
#        ───────────
#             g
#       |           |
#    e  |           |  c
#       |           |
#       |           |
#        ───────────
#             d

# Como los displays utilizados son de ánodo común:
# 0 = segmento encendido
# 1 = segmento apagado
PATRONES = {
    1: [1, 0, 0, 1, 1, 1, 1],
    2: [0, 0, 1, 0, 0, 1, 0],
    3: [0, 0, 0, 0, 1, 1, 0],
    4: [1, 0, 0, 1, 1, 0, 0],
    5: [0, 1, 0, 0, 1, 0, 0],
    6: [0, 1, 0, 0, 0, 0, 0],
}


# Se configuran los pines conectados a cada segmento
# de los displays como pines de salida
pin_a = Pin(17, Pin.OUT)
pin_b = Pin(16, Pin.OUT)
pin_c = Pin(15, Pin.OUT)
pin_d = Pin(14, Pin.OUT)
pin_e = Pin(13, Pin.OUT)
pin_f = Pin(18, Pin.OUT)
pin_g = Pin(19, Pin.OUT)


# Guarda los pines de los segmentos en una lista
# siguiendo el orden: a, b, c, d, e, f, g
seg_out = [pin_a, pin_b, pin_c, pin_d, pin_e, pin_f, pin_g]


# Pines encargados de activar cada uno de los dos displays
dig_0 = Pin(12, Pin.OUT)
dig_1 = Pin(11, Pin.OUT)


# Lista que contiene los pines de selección de los displays
dig_out = [dig_0, dig_1]


# ==================== FUNCIONES DE LOS DISPLAYS ====================

def apagar_todo():
    """
    Desactiva los dos displays.
    """

    # Recorre ambos displays y los coloca en estado apagado
    for d in dig_out:
        d.value(0)


def apagar_segmentos():
    """
    Apaga todos los segmentos de los displays.
    """

    # Al ser displays de ánodo común, un 1 apaga el segmento
    for seg in seg_out:
        seg.value(1)


def mostrar_digito(indice_display, valor):
    """
    Muestra un número en uno de los displays.
    """

    # Primero apaga ambos displays para evitar que se mezclen
    # los patrones al cambiar de un display al otro
    apagar_todo()

    # Obtiene el patrón correspondiente al número recibido
    patron = PATRONES[valor]

    # Recorre cada segmento del display
    for i, seg in enumerate(seg_out):

        # Coloca en cada segmento el valor correspondiente
        # según el patrón del número
        seg.value(patron[i])

    # Activa únicamente el display que se desea mostrar
    dig_out[indice_display].value(1)


def generar_numeros_aleatorios():
    """
    Genera dos números aleatorios entre 1 y 6
    para simular el lanzamiento de dos dados.
    """

    # Genera el resultado del primer dado
    numero_display_0 = random.randint(1, 6)

    # Genera el resultado del segundo dado
    numero_display_1 = random.randint(1, 6)

    # Devuelve ambos resultados
    return numero_display_0, numero_display_1


def mostrar_dados(numero_0, numero_1, duracion_seg=3):
    """
    Muestra los resultados de ambos dados utilizando multiplexación.
    """

    # Guarda los dos números que deben mostrarse
    valores = [numero_0, numero_1]

    # Guarda el tiempo en el que inicia la visualización
    inicio = utime.ticks_ms()

    # Mantiene los displays funcionando durante el tiempo indicado
    while utime.ticks_diff(utime.ticks_ms(), inicio) < duracion_seg * 1000:

        # Recorre los dos valores para mostrarlos de forma alternada
        for i, valor in enumerate(valores):

            # Muestra el número correspondiente en cada display
            mostrar_digito(i, valor)

            # Mantiene el display encendido durante 5 milisegundos
            # antes de cambiar al siguiente
            utime.sleep_ms(5)

    # Una vez terminado el tiempo, apaga ambos displays
    apagar_todo()

    # También apaga todos los segmentos
    apagar_segmentos()


# ==================== FUNCIONES DEL RFID ====================

def leer_tarjeta():


    lector.init()

    (stat, tag_type) = lector.request(lector.REQIDL)

    # Si no se detectó correctamente una tarjeta,
    # termina la función y devuelve None
    if stat != lector.OK:
        return None

    # Intenta obtener el UID de la tarjeta detectada
    (stat, uid) = lector.SelectTagSN()

    # Si el UID no pudo ser leído correctamente,
    # termina la función
    if stat != lector.OK:
        return None

    # Convierte el UID, que originalmente está formado por bytes,
    # en un número entero
    return int.from_bytes(bytes(uid), "little", False)


def verificar_jugador(identificador):
    """
    Verifica si el UID leído pertenece a alguno
    de los jugadores registrados.
    """

    # Busca el identificador dentro del diccionario JUGADORES.
    # Si existe, devuelve el número del jugador.
    # Si no existe, devuelve None.
    return JUGADORES.get(identificador)


# ==================== EJECUCIÓN PRINCIPAL ====================

while True:

    # Intenta leer una tarjeta RFID
    identificador = leer_tarjeta()

    # Verifica que realmente se haya detectado una tarjeta
    if identificador is not None:

        # Busca a qué jugador pertenece la tarjeta
        jugador = verificar_jugador(identificador)

        # Comprueba que la tarjeta corresponda a un jugador registrado
        if jugador is not None:

            # Genera los resultados de los dos dados
            dado_1, dado_2 = generar_numeros_aleatorios()

            # Envía mediante Serial el jugador y el resultado de los dados.
            # Ejemplo:
            # DADOS 2 4 6
            #
            # Jugador 2 obtuvo un 4 y un 6
            print(f"DADOS {jugador} {dado_1} {dado_2}")

            # Muestra los resultados en los displays durante 3 segundos
            mostrar_dados(dado_1, dado_2, duracion_seg=3)

    # Espera 200 milisegundos antes de volver a buscar una tarjeta
    utime.sleep_ms(200)