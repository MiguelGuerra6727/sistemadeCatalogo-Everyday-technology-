Create database InventarioEverydayTechnologyDB;
GO
/*NO HACER PROCEDIMIENTO MANUAL O SE ARRUINA eso dice*/
Use InventarioEverydayTechnologyDB;
/*Pruebas con code fitls*/
SELECT * FROM Roles;
SELECT * FROM Secciones;
/*PROBAR COSAS RELACIONADO A Administrador AQUI*/
SELECT u.Id, u.Nombre, u.Correo, u.Activo, r.Nombre AS Rol
FROM Usuarios u
INNER JOIN Roles r ON r.Id = u.RolId;

UPDATE Usuarios SET Correo = 'nuevo@gmail.com' WHERE Id = 1;/*Para cambiar el gmail*/
UPDATE Usuarios SET Activo = 0; /*Desactivar usuario*/
UPDATE Usuarios SET Activo = 1; /*Reactivar*/

SELECT Id, Nombre, Correo, RolId, Activo FROM Usuarios;
/*Fin*/
/*PROBAR COSAS RELACIONADO A PRODUCTOS AQUI*/

SELECT id,Codigo, Nombre, Precio, Stock, SeccionId, Activo FROM Productos;
Delete from Productos where id = 5; /*Borrar algun producto desde aqui por su id*/
SELECT Id, Nombre, Imagen FROM Secciones; /*Para ver imagenes */
SELECT id, Codigo, Nombre, Imagen FROM Productos; /*Ver si se guarda la imagen*/
SELECT id, Codigo, Nombre, Imagen FROM Productos ORDER BY id DESC;

Select * from Productos;
/*Fin*/

/*Pruebas de ventas*/

SELECT * FROM Pedidos;
SELECT * FROM PedidoItems;
SELECT id, Codigo, Nombre, Stock FROM Productos;

SELECT p.Id, p.Cliente, p.Estado, SUM(i.PrecioUnitario * i.Cantidad) AS Total
FROM Pedidos p
INNER JOIN PedidoItems i ON i.PedidoId = p.Id
GROUP BY p.Id, p.Cliente, p.Estado;

UPDATE Productos SET Stock = 0 WHERE id = 1
UPDATE Productos SET Stock = 0 WHERE id = 3

update Productos SET Stock = 20 Where id = 1
/*Fin de las pruebas*/