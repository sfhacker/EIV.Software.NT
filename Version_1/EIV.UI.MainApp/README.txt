EIV SOFTWARE NT / 2017

[2017-07-31]

* Diseno sugerido

  - Programacion en capas (DAL, BAL & UI)
  
  - Lenguaje: .NET C# [v4.5.2]


>> EIV.Software
		Type: Solution


	1.- EIV.OData.Proxy

		Contiene clases proxy provenientes del servicio OData (eivfinanciero)
		[http://192.168.1.174:8080/eivfinanciero/odata/eivfinanciero.svc/]

		Type        : Class Library
		Dependencies: Microsoft.OData.Client (6.17+)
		Used by     : Aplicacion principal (?), UI service context
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivation  : Reutilizacion

		Notes:
			Puede contener mas de una clase proxy y de diferentes lenguages (e.g. Java, .NET)
			No contiene codigo de 'usuario'
			Se actualiza facilmente (OneClick)

	
	2.- EIV.Globalization
	
		Provee servicios de traduccion entre lexicos e idiomas.
		Los lexicos soportados son: Mutual, Cooperativa y Bancos. [Revisar]
		Los idiomas soportados son: Espanol, Ingles y Portugues.  [Revisar]

		Type        : Class Library
		Dependencies: 
		Used by     : Toda la aplicacion
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivation  : Reutilizacion, encapsulamiento

		Cuando se cambia un lexico, afecta a toda la aplicacion (el mismo para todos los usuarios)
		Cuando se cambia un idioma, afecta a toda la aplicacion, pero el idioma depende de cada usuario.
		La info sobre combinacion Idioma/Usuario se persiste en algun lado? O depende de la config. de la
		PC que dicho usuario utiliza regularmente?
		A modo de ejemplo, si nuestra App soporta tres idiomas, y se utiliza el idioma seleccionado en la PC
		del usuario, y un usuario cambia el idioma de su PC por uno que nuestra App no soporta,
		como deberia reaccionar nuestra App?

	3.- EIV.TelerikThemes

        Facilita la aplicacion de distintos estilos visuales a controles de usuario.
		Soporta 15 Telerik Themes aplicable a toda la aplicacion.

	    Type        : Class Library
		Dependencies: Telerik Themes
		Used by     : Toda la aplicacion
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivation  : Reutilizacion

        Cuando se cambia un tema, afecta a toda la aplicacion, pero el tema depende de cada usuario.
		Donde/Como se almacena dicha info?


	4.- EIV.Helpers
	
		Type        : Class Library
		Dependencies: Varios (e.g. JSon, XML, ...)
		Used by     : Toda la aplicacion
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivation  : Reutilizacion


	5.- EIV.UI.UserControlBase

		Clase base para la creacion de formularios y ventanas.

	    Type        : Class Library
		Dependencies: WPF + Telerik
		Used by     : Aplicacion principal
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivation  :


	6.- EIV.UI.Formularios
	
		Agrupa a todos los formularios ofrecido x la aplicacion.
		
		Type        : Class Library
		Dependencies: WPF + Telerik
		Used by     : Aplicacion principal
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivation  : Agregar, modificar formularios de manera independiente del resto de la aplicacion.
		              principalmente post deployment a produccion.


	7.- EIV.UI.ServiceContext

	    Type        : Class Library
		Dependencies: WPF + Telerik
		Used by     : Aplicacion principal
		Deployment  : Un solo assembly
		Tests       : N/A

		Motivacion  : Conexion al servicio OData, busqueda de formularios, metodos varios.

		Notas       : Esto puede ser overkill y eventualmente, convertirse en un DLL gigantesco!
		              Pero al mismo tiempo se pretende limitar al minimo las dependencias entre
					  proyectos. La idea seria que el ejecutable tenga este assembly unicamente
					  como dependenica (es factible? deseable? pretensioso?)

	8.- EIV.UI.MainApp
	
		Type        : WPF Application
		Dependencies: WPF + Telerik
		Used by     : N/A
		Deployment  : Ejecutable + dependencias
		Tests       : N/A
	
	
* Funcionamiento / Requerimientos / Dependencias

	- Revisar las dependencias entre proyectos

	- Revisar el diseno de la solucion (e.g. capas)

	- Como organizar el menu de opciones principal. Al presente, el menu principal contiene
	  un numero (en algun caso) y letra (en otro caso) delante del nombre de la opcion del menu.
	  Se podria organizar alfabeticamente con algunas opciones fijas (por ejemplo)
	  El menu de opciones depende del usuario logeado, del grupo al que dicho usuario pertence,
	  o de ambos casos, o .... ? Si un usuario pertenece a multiples grupos, como funciona el menu?
	  Hay un grupo primario/default y ......
	  En otro caso, el menu utiliza lo que el servicio devuelve?

	- Seria util compilar un doc que incluya feedback provisto x los usuarios durante el soporte
	  Este feedback puede incluir aspectos visuales, usabilidad de la interfaz/app, funcionalidad, 
	  facilidad para su mantenimiento, etc., etc.

* TODO

	- La version automatica del DataForm parece no satisfacer nuestros requerimientos 
	  (los mensajes de error no son localizables, un textbox para un campo nrico. necesita un 0,
	   cuando se muestra una propiedad de navegacion aparece el tipo de dato, etc.)