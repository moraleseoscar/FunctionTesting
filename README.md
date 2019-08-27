# Testeo-de-funciones
Un programa para poner bajo estrés de cerca de 1,500,000 datos procesados para tres funciones "List", "Split" y "subString"


Lo primero que se tiene que hacer en esta parte es abrir el query adjunto en la carpeta y ejecutarlo para crear la base de datos.

Después de generar la base de datos iremos al archivo "Program.cs" y empezaremos a colocar la linea de conexión a nuestra base de datos donde vamos a ingresar los datos a testear.

Exactamente en esta línea:

string cadenaConexion = @"Data Source=SUINSTANCIA;Initial Catalog=SUDB;User ID=SUUSUARIO; Password=SUCONTRASEÑA; Connect Timeout=60";

No aseguramos de rellenar los campos que se encuentran en mayúsculas y luego ejecutamos el programa, si esté se ejecuto correctamente nos solicitará una ruta de donde se encuentra nuestro archivo, por ejemplo: "D:\Usuarios\Oscar\Escritorio\Prueba\Registros.csv" es importante colocarle la extensión al archivo e ingresar el texto como se muestra.

Al momento que el programa se ejecute iniciara un conteo, aproximadamente 10-30 segundos fue el promedio que se obtuvo por cada uno de los procesos en nuestro caso, puede que este tiempo varie, pero ninguno puede durar más de 60s en ese caso puede ser que su conexión a la base de datos la haya colocado mal. 

Cuando el programa se ejecute presentara un texto como el siguiente:

"Time elapsed: 07.1254s"

Cuando aparesca este texto será necesario presionar la tecla ENTER para pasar a la siguiente función, al final obtendrá los datos de las 3, y si miramos la base de datos, tendríamos que tener cerca de 15,000 registros ingresados.
