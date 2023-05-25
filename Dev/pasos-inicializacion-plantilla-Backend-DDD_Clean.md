# Pasos para ejecutar plantilla DDD y Clean correctamente
*configurar parametros en appsettings o en las librarys para cloud:
  AppSettings
		DefaultCountry: co
		DomainName: co
		StorageContainerName: cpc-bst-dev-eu2
		Database: DBPlantilla
		HealthChecksEndPoint: /health
	Secrets
		ServiceBusConnection: credinet-servicesbus-dev
	DataBase 
		ConnectionString: Server=credinet-qa-sqlsvr.database.windows.net;Database=Poc;User ID =AdminSistecreditoQa;Pwd =S04-v3ry-53cr37-p455w0rd
	AzureKeyVaultConfig: 
		TenantId: e8361441-1aea-47f0-b237-7461832d615f
		AppId: 8b231cf6-9e8e-423e-95fa-dbbfefc3eeb8
		AppSecret: l+vAkJBjD/Y54Ib3UTegeOchxcEy1XYIL8Rm13XuiTc=
		KeyVault: https://credinetkeyvault.vault.azure.net
	MongoConfigurationProvider: 
		ConnectionString: credinet-mongodb-dev
		CollectionName: ConfigTemplateTest
		DatabaseName: DBConfig_co
		ReloadOnChange: true
*configuracion mongo provider
 *ingresar al ambiente de desarrollo: mongodb://dbCredinetDev2019:SisteCredito2019123@credinet2019development-shard-00-02-0n9nb.azure.mongodb.net:27017,credinet2019development-shard-00-00-0n9nb.azure.mongodb.net:27017,credinet2019development-shard-00-01-0n9nb.azure.mongodb.net:27017/DBConfig_co?3t.connection.name=CrediNet2019Development-shard-0&3t.databases=admin%2Ctest&3t.uriVersion=3&authMechanism=SCRAM-SHA-1&authSource=admin&connectTimeoutMS=10000&replicaSet=CrediNet2019Development-shard-0&ssl=true
 *verificar que exista la colección "ConfigTemplateTest" en la base de datos "DBConfig_co"
 *ejecutar el siguiente script "script_mongo.txt" en caso de que no exista la colección "ConfigTemplateTest"
*configuracion sql server
 *ingresar al siguiente ambiente: Server:credinet-qa-sqlsvr.database.windows.net, Login: AdminSistecreditoQa , Password: S04-v3ry-53cr37-p455w0rd
 *verificar que exista la BD's "Poc"
 *ejecutar el siguiente script "script_sql.sql" en caso de que no exista la base de datos
*conexion serviceBus desarrollo: Endpoint=sb://sccredinetservicebus.servicebus.windows.net/;SharedAccessKeyName=SC_servicebus_authrule;SharedAccessKey=VEwtzu2HnfY3Qci+r1yi78j+zTAEKLWiZELD+3ajdkI=
 *crear la cola request_"createNote_queue_local"
 *crear el topic "request_createNote_topic_local" y subscription "msv-ddd"
 
*NOTA: los parametros anteriormente dejados como conectionString de mongo, sql y serviceBus está sujeto a cambio y manejo de cada equipo
 