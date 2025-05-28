
DROP TABLE #t1
select tm_riferim,count(*) as n
into #t1
from testmag
where tm_riferim like 'PRELIE%'
and tm_datdoc>='04/02/2024'
group by tm_riferim
having count(*)>1



select t.tm_ultagg,m.mm_ultagg,t.tm_opnome, m.mm_opnome, m.mm_codart,m.mm_quant, * from testmag as t
inner join #t1 on #t1.tm_riferim=t.tm_riferim
inner join movmag as m on m.mm_tipork = t.tm_tipork
and m.mm_anno = t.tm_anno
and m.mm_serie = t.tm_serie
and m.mm_numdoc = t.tm_numdoc
order by t.tm_riferim


select tm_riferim,count(*) as n
from testmag 

where tm_riferim like 'PRELIE%'
and tm_datdoc>='04/02/2024'
group by tm_riferim
having count(*)>1



select row_number() OVER (PARTITION BY t.tm_riferim order by t.tm_riferim) as r1, * from testmag as t
inner join #t1 on #t1.tm_riferim=t.tm_riferim
order by t.tm_riferim


select REPLACE(tm_note,'fiscale','automatico'),* 

from testmag where tm_serie='M' and tm_note like '20.03.2024%'

SELECT tm_numdoc,tm_riferim,tm_note FROM testmag where tm_riferim like '##%'