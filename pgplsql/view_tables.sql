CREATE OR REPLACE VIEW iew_lista_festas AS
SELECT
	c.nome_cliente,
	a.nome_aniversariante,
	f.data_festa,
	f.hora_festa,
	f.tema,
	f.convidados,
	f.pacote,
	p.valor_final
FROM Festa f
INNER JOIN Cliente c ON f.id_cliente = c.id_cliente
INNER JOIN Aniversariante a ON f.id_aniversariante = a.id_aniversariante
INNER JOIN PagamentoPrev p ON f.id_pagamento = p.id_pagamento;