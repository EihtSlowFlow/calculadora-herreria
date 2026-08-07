# Plan del prototipo — Inventario y calculadora de cortes para herrería

## 1. Objetivo

Construir una aplicación de escritorio para Windows, local y monousuario, que permita al cliente:

- consultar y registrar materiales;
- calcular el aprovechamiento de barras y perfiles;
- combinar cortes de distintas medidas;
- calcular desarrollos circulares a partir de un diámetro;
- mantener una calculadora compacta visible mientras trabaja en FreeCAD, AutoCAD u otro programa;
- conocer el material utilizado, la pérdida producida por el corte y el sobrante resultante.

El propósito del prototipo es validar las reglas de cálculo y la forma de trabajo del cliente antes de desarrollar funciones avanzadas.

## 2. Plataforma y modalidad

- Aplicación de escritorio independiente para Windows.
- Uso local en una sola computadora.
- Un único usuario, sin cuentas ni permisos.
- Funcionamiento sin conexión a Internet.
- Base de datos local.
- Sin integración directa con FreeCAD o AutoCAD en esta etapa.
- Tecnología propuesta: C#/.NET con WPF y SQLite.

## 3. Alcance incluido

### 3.1 Inventario básico

El prototipo permitirá registrar dos grupos de materiales.

**Materiales lineales**

- Ángulo L.
- Perfil T.
- Planchuela.
- Tubo redondo.
- Tubo cuadrado.
- Tubo rectangular.
- Otros tipos definidos por el usuario.

Datos mínimos:

- nombre o descripción;
- familia y forma;
- material;
- dimensiones y espesor;
- unidad original;
- largo disponible;
- cantidad;
- condición: barra completa o sobrante;
- observaciones.

Las barras nuevas tendrán 6000 mm como valor predeterminado, pero el largo podrá modificarse.

**Chapas y placas**

En el prototipo se podrán registrar mediante:

- largo;
- ancho;
- espesor;
- cantidad;
- condición: completa o sobrante.

El área se calculará automáticamente. La optimización visual de cortes en dos dimensiones no estará incluida.

### 3.2 Calculadora flotante

Ventana compacta e independiente con:

- opción «Siempre visible»;
- posibilidad de moverla, redimensionarla y minimizarla;
- conservación de su posición y tamaño;
- selector de unidades: mm, cm, m y pulgadas;
- uso de coma decimal en pantalla;
- botón para copiar resultados;
- acceso desde la aplicación principal.

Tendrá tres pestañas.

#### A. Corte simple

Entradas:

- largo de la barra;
- largo de la pieza;
- cantidad deseada, opcional;
- ancho de corte o *kerf*;
- margen o tolerancia adicional, inicialmente opcional.

Resultados:

- piezas obtenibles por barra;
- cantidad de barras necesarias, si se indicó una cantidad deseada;
- cantidad de cortes;
- longitud útil convertida en piezas;
- material consumido por los cortes;
- sobrante por barra;
- sobrante total.

#### B. Secuencia o plan de cortes

Permitirá agregar varias medidas y cantidades a un mismo cálculo.

Ejemplo de una unidad de producto:

| Cantidad | Largo | Uso |
|---:|---:|---|
| 2 | 400 mm | Lados cortos |
| 2 | 600 mm | Lados largos |
| 4 | 125 mm | Patas |

El usuario podrá indicar la cantidad de productos por fabricar. Para 40 unidades, el sistema transformará el pedido en:

- 80 piezas de 400 mm;
- 80 piezas de 600 mm;
- 160 piezas de 125 mm.

Después calculará:

- barras necesarias;
- distribución sugerida de piezas por barra;
- secuencia de cortes;
- pérdida total por la herramienta;
- sobrante de cada barra;
- resumen total de piezas.

El prototipo podrá guardar la combinación como una plantilla o «modelo de fabricación» para reutilizarla.

#### C. Desarrollo circular

Entradas:

- diámetro;
- unidad;
- margen adicional para unión, soldadura o solapamiento;
- cantidad de piezas.

Cálculo principal:

\[
L = D \times \pi + M
\]

Donde:

- \(L\) es la longitud final;
- \(D\) es el diámetro;
- \(M\) es el margen adicional.

Resultados:

- circunferencia calculada;
- margen agregado;
- longitud final de cada pared;
- longitud total para la cantidad solicitada;
- opción para enviar esa longitud a la calculadora de barras.

### 3.3 Confirmación del consumo

Los cálculos no modificarán automáticamente el inventario. El usuario deberá seleccionar «Confirmar consumo».

Al confirmar:

- se descontarán las barras utilizadas;
- se registrarán los sobrantes reutilizables;
- se guardará un movimiento básico con fecha, material y cantidades;
- se permitirá cancelar antes de aplicar el cambio.

## 4. Alcance excluido del prototipo

- Integración o extensión para AutoCAD o FreeCAD.
- Lectura automática de medidas desde archivos CAD.
- Optimización bidimensional de chapas y placas.
- Distribución de figuras irregulares.
- Usuarios, contraseñas y permisos.
- Sincronización entre computadoras.
- Aplicación móvil o versión web.
- Compras, proveedores, facturación y presupuestos.
- Códigos de barras o códigos QR.
- Cálculos avanzados de plegado, deformación o línea neutra.
- Gestión completa de estaciones de producción.

## 5. Reglas de cálculo iniciales

Todos los valores se convertirán internamente a milímetros. Las unidades originales se conservarán para mostrarlas al usuario.

Para la primera validación se asumirá que:

- cada pieza separada requiere un corte;
- cada corte consume el ancho indicado por el usuario;
- no existe despunte inicial salvo que posteriormente se agregue como parámetro;
- el resultado no puede superar el largo disponible;
- los resultados se mostrarán con hasta dos decimales;
- un sobrante solo volverá al inventario si supera una medida mínima configurable.

Para una barra de longitud \(B\), piezas iguales de longitud \(P\) y ancho de corte \(K\), la condición de validez será:

\[
n(P+K) \leq B
\]

Por ejemplo, con 6000 mm, piezas de 370 mm y un corte de 3 mm:

- 16 piezas;
- 48 mm consumidos por 16 cortes;
- 32 mm de sobrante.

La distribución de múltiples medidas utilizará inicialmente un algoritmo de optimización unidimensional que busque reducir la cantidad de barras y producir sobrantes aprovechables. El resultado deberá mostrarse como una sugerencia editable, no como una orden irreversible.

## 6. Pantallas del prototipo

### Pantalla 1 — Inicio e inventario

- listado y búsqueda de materiales;
- filtros por familia y forma;
- cantidad de barras completas y sobrantes;
- botones «Nuevo material», «Ingreso», «Ajuste» y «Abrir calculadora».

### Pantalla 2 — Formulario de material

- datos descriptivos;
- medidas y unidades;
- largo o dimensiones de placa;
- cantidad y observaciones.

### Pantalla 3 — Calculadora flotante

- pestañas Corte simple, Plan de cortes y Circular;
- resultados inmediatos;
- botón de fijación sobre otras ventanas;
- copiar, limpiar, guardar plantilla y confirmar consumo.

### Pantalla 4 — Resultado del plan

- cantidad de barras;
- representación lineal sencilla de cada barra;
- piezas asignadas en orden;
- pérdidas y sobrantes;
- advertencias si no existe material suficiente.

### Pantalla 5 — Plantillas de fabricación

- nombre del producto o conjunto;
- piezas requeridas por unidad;
- cantidad de unidades por fabricar;
- edición y reutilización.

### Pantalla 6 — Movimientos básicos

- fecha;
- material;
- tipo de movimiento;
- cantidad anterior y posterior;
- sobrante registrado.

## 7. Etapas de construcción

### Etapa 1 — Validación funcional

- revisar dos o tres casos reales con el cliente;
- confirmar la forma de contar cortes y pérdidas;
- definir el sobrante mínimo reutilizable;
- acordar unidades, redondeos y tolerancias;
- dibujar bocetos de las pantallas.

**Resultado:** reglas aprobadas y boceto navegable.

### Etapa 2 — Motor de cálculos

- conversión de unidades;
- corte simple;
- desarrollo circular;
- multiplicación de piezas por cantidad de productos;
- distribución unidimensional de múltiples medidas;
- pruebas automatizadas con casos conocidos.

**Resultado:** cálculos verificables sin depender de la interfaz.

### Etapa 3 — Ventana flotante

- construcción de las tres pestañas;
- comportamiento «Siempre visible»;
- copiar resultados;
- recordar posición, tamaño y últimos parámetros;
- validación junto a FreeCAD y AutoCAD.

**Resultado:** calculadora utilizable durante el trabajo cotidiano.

### Etapa 4 — Inventario local

- catálogo de materiales;
- barras completas y sobrantes;
- movimientos de entrada y consumo;
- conexión entre cálculo y confirmación de consumo;
- persistencia local.

**Resultado:** circuito básico completo desde el material disponible hasta el sobrante.

### Etapa 5 — Prueba con el cliente

- ejecutar trabajos reales;
- comparar los resultados con sus cálculos manuales;
- observar el uso de la ventana flotante;
- registrar confusiones y excepciones;
- corregir fórmulas e interfaz.

**Resultado:** prototipo validado o lista priorizada de cambios.

### Etapa 6 — Entrega técnica

- generar instalador para Windows;
- agregar copia de seguridad manual de la base local;
- preparar instrucciones breves;
- entregar una versión identificada del prototipo.

## 8. Estimación inicial

Una estimación razonable para una sola persona desarrolladora es de 15 a 20 días hábiles:

| Trabajo | Estimación |
|---|---:|
| Entrevista, reglas y bocetos | 1–2 días |
| Motor de cálculos y pruebas | 3–4 días |
| Ventana flotante | 3–4 días |
| Inventario y persistencia | 3–4 días |
| Plantillas y plan de cortes | 2–3 días |
| Prueba, correcciones e instalador | 3 días |

La estimación deberá revisarse después de validar casos reales, especialmente la optimización de múltiples medidas.

## 9. Criterios de aceptación

El prototipo se considerará funcional cuando:

1. Permita registrar un material lineal y su stock disponible.
2. Calcule correctamente piezas iguales, pérdida por corte y sobrante.
3. Calcule cuántas barras se necesitan para una cantidad solicitada.
4. Admita varias medidas y cantidades en un mismo plan.
5. Multiplique una plantilla de fabricación por la cantidad de productos.
6. Calcule una circunferencia a partir del diámetro y un margen opcional.
7. Convierta correctamente entre mm, cm, m y pulgadas.
8. Mantenga la calculadora visible sobre AutoCAD o FreeCAD cuando el usuario lo active.
9. Conserve inventario, plantillas y configuración después de reiniciar.
10. No descuente material hasta recibir confirmación explícita.
11. Registre como sobrante el material aprovechable.
12. Muestre mensajes claros ante valores inválidos o falta de stock.

## 10. Preguntas pendientes para cerrar antes de programar

1. ¿Cada pieza se contará siempre como un corte o existen casos con un corte menos?
2. ¿Se empareja el extremo de la barra antes de comenzar y cuánto material se pierde?
3. ¿Cuál es la longitud mínima para conservar un sobrante?
4. ¿Qué tolerancia se agrega a las medidas y cómo debe redondearse?
5. ¿Los cortes serán únicamente rectos o también se deben registrar ángulos?
6. ¿El resultado debe poder imprimirse como lista de cortes para entregarlo al operario?
7. ¿El cliente quiere modificar manualmente la distribución propuesta?
8. ¿Debe guardarse el resultado calculado, el resultado real o ambos?

## 11. Próxima acción recomendada

Realizar una entrevista corta usando dos trabajos reales:

- uno con piezas repetidas de una sola medida;
- otro con un producto compuesto por varias medidas.

Con esos casos se completarán las respuestas pendientes y se convertirán en pruebas de aceptación antes de desarrollar la interfaz.
