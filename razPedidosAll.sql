-- yjs 240719 ufff!
 create  view VistaPedidosApi as  
    SELECT cast(a.num as int) num, clien1.direcion, 
    cast(dbo.fDeterminarReparto(clien1.reparto, clien1.reparto2, a.idcanalpedido, 
    case when clien1.vendedor3<>0 then clien1.reparto2 else clien1.reparto end, a.linea) as int) as reparto,    
     case when a.tipofac = 2 then 'NC' else '' end as tipofac ,  
     case when a.identif = 'G' and a.linea = 109 then 'P' else a.identif end as identif,
     0 as marca, a.username,isnull(a.impreso,'') impreso ,a.codcli,convert(date,a.fechaf) fechaf,convert(date,a.fechacuando) fechacuando,    
     clien1.bloqueada, clien1.bloqueada2, clien1.recontrainhab,
     case when isnull(a.cf,0) = 1 then '*' else '' end as cf     
     FROM     
        (SELECT DISTINCT pedidos.num, codcli, fechaf, tipofac, b.linea,     
        case when b.vtaespecial = 1 then 'E' when l.marcabebidas = 1 then 'B' else identif end as identif, username,     
        isnull(impreso,'') as impreso,    
        paracuando, mostrador, idgrupofacturacionespecial, fechacuando, idCanalPedido, count(*) as cantart, sum(cantd) as totunid, cf     
        FROM pedidos     
        INNER JOIN    
         (select num, min(codigo) as codigo from pedidos group by num) x1 ON pedidos.num = x1.num    
        INNER JOIN articulo b ON x1.codigo = b.codigo INNER JOIN lineas l ON b.linea = l.codigo WHERE identif = 'G' AND username<>'OMAR'    
        AND username<>'SOLANO' AND username<>'RAUL' AND tipofac = 1 and idusuariopedido not in (98, 153, 302, 314, 267, 188, 64, 345)    
        group by pedidos.num, codcli, fechaf, tipofac, case when b.vtaespecial = 1 then 'E' when l.marcabebidas = 1 then 'B' else identif end,    
        username, impreso, paracuando, mostrador, idgrupofacturacionespecial, fechacuando, idCanalPedido, b.linea, cf ) a     
     INNER JOIN clien1 ON a.codcli = clien1.codcli    
     INNER JOIN repartos ON dbo.fDeterminarReparto(clien1.reparto, clien1.reparto2, a.idcanalpedido,    
     case when clien1.vendedor3 <> 0 then clien1.reparto2 else clien1.reparto end, a.linea) = repartos.codigo   			

--select * from VistaPedidosApi order by num

create procedure retorna_pedido (@idPedido int) as  
  
SELECT num,codigo,descrip,cantd,
cast (cajasc as int) cajasc,precio,username,idsector from  
(select articulo.descrip,pedidos.*, 00000 AS ordenlin, ISNULL(o.orden,0) as ordenart, dbo.fnStockParaPedido(pedidos.codigo, 1) as stock2,  
 ISNULL(oap2.orden, ISNULL(oap.orden, 0)) as ordenimp,  
 ISNULL(b.idSector, 'Z. SIN DEFINIR') as idSector, ISNULL(b.idPasillo, '') as idPasillo,   
 ISNULL(b.idLadoPasillo, '') as idLadoPasillo, ISNULL(b.idColumna, '') as idColumna, ISNULL(b.idNivel, '') as idNivel   
 FROM pedidos  
  LEFT JOIN pedidosalimentosordenimpresion o ON pedidos.codigo=o.idarticulo   
  inner join articulo4 on pedidos.codigo=articulo4.codigo   
  inner join articulo on pedidos.codigo=articulo.codigo   
  left join ordenarmadopedidos oap   
  on articulo.linea = oap.idlinea and articulo.rubro = oap.idrubro and oap.idarticulo is null and oap.idsucursal = 1  
   left join ordenarmadopedidos oap2 on articulo.linea = oap2.idlinea and articulo.rubro = oap2.idrubro and articulo.codigo = oap2.idarticulo and   
   oap2.idsucursal = 1 left join   
       ( select *, ROW_NUMBER() over (partition by idarticulo order by iddeposito, idsector, idpasillo, idladopasillo, idcolumna, idnivel) as queorden   
            from inv.BinesArticulos ) b on pedidos.codigo = b.idArticulo and b.idDeposito = 1 and b.queorden = 1 WHERE pedidos.num=  @idPedido  
            and articulo4.sucursal=1) bdd  

alter procedure retorna_pedido (@idPedido int) as --sum(cantd) cantd
select num,codigo,descrip,username,idsector,sum(cantd) cantd,sum(cajasc) cajasc from
(SELECT num,codigo,descrip,cantd,convert(int,cajasc) cajasc, username,idsector from
(select articulo.descrip,pedidos.*, 00000 AS ordenlin, ISNULL(o.orden,0) as ordenart, dbo.fnStockParaPedido(pedidos.codigo, 1) as stock2,
 ISNULL(oap2.orden, ISNULL(oap.orden, 0)) as ordenimp,
 ISNULL(b.idSector, 'Z. SIN DEFINIR') as idSector, ISNULL(b.idPasillo, '') as idPasillo, 
 ISNULL(b.idLadoPasillo, '') as idLadoPasillo, ISNULL(b.idColumna, '') as idColumna, ISNULL(b.idNivel, '') as idNivel 
 FROM pedidos
  LEFT JOIN pedidosalimentosordenimpresion o ON pedidos.codigo=o.idarticulo 
  inner join articulo4 on pedidos.codigo=articulo4.codigo 
  inner join articulo on pedidos.codigo=articulo.codigo 
  left join ordenarmadopedidos oap 
  on articulo.linea = oap.idlinea and articulo.rubro = oap.idrubro and oap.idarticulo is null and oap.idsucursal = 1
   left join ordenarmadopedidos oap2 on articulo.linea = oap2.idlinea and articulo.rubro = oap2.idrubro and articulo.codigo = oap2.idarticulo and 
   oap2.idsucursal = 1 left join 
       (	select *, ROW_NUMBER() over (partition by idarticulo order by iddeposito, idsector, idpasillo, idladopasillo, idcolumna, idnivel) as queorden 
   	        from inv.BinesArticulos ) b on pedidos.codigo = b.idArticulo and b.idDeposito = 1 and b.queorden = 1 WHERE pedidos.num=   @idPedido
   	        and articulo4.sucursal=1) bdd) def
  group by num,codigo,descrip,username,idsector 	        

exec retorna_pedido 10238897

--drop procedure retorna_pedido




CREATE procedure spArticuloBarras (@idProductos varchar(1000)) spAddCola @user,@idpedido,@numitem,@iadrticulo,@idsector,@cantidad,@status,@FechaStatus,@pickeado,@cantidadPick
as  
declare @cCodigo varchar (50)
--declare @idProductos varchar(1000)
--set @idProductos = '12,'
DECLARE @cnt INT = 0;
--if exists (select * from #tempBarras)
--	drop table #tempBarras;
--else	
create table #tempBarras (codigo int,CodigoBarra varchar(20),factor int,bultos int,cajas int, unidades int);
WHILE 1 = 1
BEGIN
    set @cCodigo = ''
    WHILE (1 = 1)
    BEGIN
	    SET @cnt = @cnt + 1;

		if substring(@idProductos,@cnt,1) = ','
		begin
			break
		end;	
		else
		begin
			set @cCodigo = @cCodigo + substring(@idProductos,@cnt,1);	
		end;	
    END      
	insert into  #tempBarras 
	select ca.IdArticulo codigo,
	ca.CodigoBarra, min(ca.Factor) as Factor, min(Bultos) as Bultos, min(Cajas) as Cajas, min(Unidades) as Unidades  
	from   
	(  
	select   
	 a.codigo as IdArticulo,
	 sb.cbunidad as CodigoBarra,  
	 1 as Factor,  
	 0 as Bultos,  
	 0 as Cajas,  
	 1 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbunidad<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbbulto as CodigoBarra,  
	 a.cajapor * a.bulto as Factor,  
	 1 as Bultos,  
	 0 as Cajas,  
	 0 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbbulto<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbcaja as CodigoBarra,  
	 a.cajapor as Factor,  
	 0 as Bultos,  
	 1 as Cajas,  
	 0 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbcaja<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbunidad2 as CodigoBarra,  
	 1 as Factor,  
	 0 as Bultos,  
	 0 as Cajas,  
	 1 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbunidad2<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbbulto2 as CodigoBarra,  
	 a.cajapor * a.bulto as Factor,  
	 1 as Bultos,  
	 0 as Cajas,  
	 0 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbbulto2<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbcaja2 as CodigoBarra,  
	 a.cajapor as Factor,  
	 0 as Bultos,  
	 1 as Cajas,  
	 0 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbcaja2<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbunidad3 as CodigoBarra,  
	 1 as Factor,  
	 0 as Bultos,  
	 0 as Cajas,  
	 1 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbunidad3<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbbulto3 as CodigoBarra,  
	 a.cajapor * a.bulto as Factor,  
	 1 as Bultos,  
	 0 as Cajas,  
	 0 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbbulto3<>''  
	union  
	select   
	 a.codigo as IdArticulo,  
	 sb.cbcaja3 as CodigoBarra,  
	 a.cajapor as Factor,  
	 0 as Bultos,  
	 1 as Cajas,  
	 0 as Unidades  
	from Solano_Barras sb  
	inner join articulo a on a.codigo=sb.idArticulo and a.inactivo=0  
	where  
	 sb.cbcaja3<>''   
	) as ca 
	where ca.IdArticulo IN (@cCodigo)--where  TRIM(cCodigo) ='5187'	
	group by   ca.IdArticulo,ca.CodigoBarra

	--where  ca.IdArticulo in (@idProductos)
	if @cnt >= len(trim(@idProductos))
		break;
END;
select *  from #tempBarras 
drop table #tempBarras 


--exec spArticuloBarras '12,'

--exec spArticuloBarras '2461,1,5267,12,12015,'

create table PedidosCompletados (idPedidosCompletados int identity,
								num int,
								codigo int,
								cantrec int,
								cajasrec int,
								bultosrec int,
								idusuario int,
								fecha datetime
								)
exec spAddCola 1,0,1,9618923,'Z. SIN DEFINIR',3,1,'2019-07-25 15:19:47.000',3,0
create table ColaArmadoPedidos (idColaArmadoPedidos int identity,
								idUsuario int,
								idPedido int,
								numItem int,
								idArticulo int,
								idSector varchar(50),
								cantidad int,
								status int,
								fechaStatus datetime,
								pickeado int)

----******

alter table pedidos add  procesado bit

----*****
use pruebas
exec spAddCola 1,10238897,1,5237,'Z. SIN DEFINIR',3,1,'2019-07-25 15:19:47.000',3,0

alter procedure spAddCola (@user int ,@idpedido int,@numitem int,@iadrticulo int,
	@idsector varchar(50),@cantidad int ,@status int,@FechaStatus datetime,@pickeado int,@cantidadPick int)
as
if not exists(select top 1 idusuario from colaarmadopedidos where idPedido = @idpedido and idArticulo = @iadrticulo) 
begin 
insert into ColaArmadoPedidos (idUsuario,idPedido,numItem,idArticulo,idSector,cantidad,status,FechaStatus,pickeado) values 
(@user,@idpedido,@numitem,@iadrticulo,@idsector,@cantidad,@status,@FechaStatus,@pickeado)
update pedidos set procesado = 1 where num = @idpedido and codigo = @iadrticulo 
 end ;
	
	
	
	
	delete from ColaArmadoPedidos
	
	
	exec retorna_pedido 10238897
	
	
	select top 100 * from cccte
	
7798130959406       
select "7798130959406       " = "7798130959406       "7798130959406       

SELECT * FROM ColaArmadoPedidos

exec spArticuloBarras '8531,'