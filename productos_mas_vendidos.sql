SELECT ventas_detalle.id_producto, nombre_producto, COUNT(ventas_detalle.id_producto) as cantidad 
FROM productos
INNER JOIN ventas_detalle
USING (id_producto) 
GROUP BY ventas_detalle.id_producto
HAVING cantidad = (
    SELECT COUNT(ventas_detalle.id_producto) 
    FROM ventas_detalle 
    INNER JOIN productos 
    USING (id_producto) 
    GROUP BY ventas_detalle.id_producto 
    ORDER BY COUNT(ventas_detalle.id_producto) DESC 
    LIMIT 1)
ORDER BY cantidad;

--WHERE no se puede aplicar a una función de agregación, pero si el HAVING