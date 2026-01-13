import { useMemo, useState } from "react";
import { useFinLancamentos_BKPs, useDeleteFinLancamentos_BKP } from "@/lib/abp/hooks/useFinLancamentos_BKPs";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Search, MoreHorizontal, Pencil, Trash2, Loader2 } from "lucide-react";
import { toast } from "sonner";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

interface FinLancamentos_BKPListProps {
  onEdit: (item: any) => void;
}

export function FinLancamentos_BKPList({ onEdit }: FinLancamentos_BKPListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useFinLancamentos_BKPs({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFinLancamentos_BKP();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this finlancamentos_bkp?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("finLancamentos_BKP deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete finlancamentos_bkp");
      }
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center p-12">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  return (
    <Card>
      <CardContent className="p-0">
        <div className="p-4 border-b">
          <div className="relative max-w-sm">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search finlancamentos_bkps..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idLancamento</TableHead>
              
              <TableHead>idConta</TableHead>
              
              <TableHead>idPlanoConta</TableHead>
              
              <TableHead>idCentroCusto</TableHead>
              
              <TableHead>operacao</TableHead>
              
              <TableHead>idForma</TableHead>
              
              <TableHead>modulo</TableHead>
              
              <TableHead>idCadastro</TableHead>
              
              <TableHead>idPedido</TableHead>
              
              <TableHead>descricao</TableHead>
              
              <TableHead>nrDocumento</TableHead>
              
              <TableHead>valor</TableHead>
              
              <TableHead>dataEmissao</TableHead>
              
              <TableHead>dataVencimento</TableHead>
              
              <TableHead>dataQuitacao</TableHead>
              
              <TableHead>quitado</TableHead>
              
              <TableHead>recorrente</TableHead>
              
              <TableHead>recorrenteChave</TableHead>
              
              <TableHead>previsao</TableHead>
              
              <TableHead>cobrancaEnviada</TableHead>
              
              <TableHead>parcelado</TableHead>
              
              <TableHead>identificacao</TableHead>
              
              <TableHead>observacao</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>idUsuarioInclusao</TableHead>
              
              <TableHead>idUsuarioAlteracao</TableHead>
              
              <TableHead>parcela</TableHead>
              
              <TableHead>parcelaMaxima</TableHead>
              
              <TableHead>dataVencimentoOriginal</TableHead>
              
              <TableHead>pagtoLiberado</TableHead>
              
              <TableHead>dataParaPrevisao</TableHead>
              
              <TableHead>recorrenteVencendoVisto</TableHead>
              
              <TableHead>recebimentoFuturo</TableHead>
              
              <TableHead>recebimentoFuturoRel</TableHead>
              
              <TableHead>idTerceiro</TableHead>
              
              <TableHead>arquivoDocumento</TableHead>
              
              <TableHead>arquivoComprovante</TableHead>
              
              <TableHead>idClientePagar</TableHead>
              
              <TableHead>idProcessoPagar</TableHead>
              
              <TableHead>idArea</TableHead>
              
              <TableHead>identificacaoPagar</TableHead>
              
              <TableHead>identificacaoPagar2</TableHead>
              
              <TableHead>arquivoDocumento2</TableHead>
              
              <TableHead>arquivoComprovante2</TableHead>
              
              <TableHead>verba</TableHead>
              
              <TableHead>verbaDataDe</TableHead>
              
              <TableHead>verbaDataAte</TableHead>
              
              <TableHead>verbaEstado</TableHead>
              
              <TableHead>verbaCidade</TableHead>
              
              <TableHead>idCentroResultado</TableHead>
              
              <TableHead>secundaria</TableHead>
              
              <TableHead>geradoPeloProcesso</TableHead>
              
              <TableHead>sequenciaHerdeiro</TableHead>
              
              <TableHead>idUnidade</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idLancamento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idPlanoConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCentroCusto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.operacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idForma}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.modulo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCadastro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idPedido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descricao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nrDocumento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataEmissao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataVencimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataQuitacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.quitado ? "default" : "secondary"}>
                    {item.quitado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recorrente ? "default" : "secondary"}>
                    {item.recorrente ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recorrenteChave}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.previsao ? "default" : "secondary"}>
                    {item.previsao ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.cobrancaEnviada ? "default" : "secondary"}>
                    {item.cobrancaEnviada ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.parcelado ? "default" : "secondary"}>
                    {item.parcelado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.identificacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.observacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.ativo ? "default" : "secondary"}>
                    {item.ativo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsInclusao ? new Date(item.tsInclusao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsAlteracao ? new Date(item.tsAlteracao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idUsuarioInclusao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idUsuarioAlteracao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.parcela}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.parcelaMaxima}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataVencimentoOriginal}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.pagtoLiberado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataParaPrevisao}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recorrenteVencendoVisto ? "default" : "secondary"}>
                    {item.recorrenteVencendoVisto ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebimentoFuturo ? "default" : "secondary"}>
                    {item.recebimentoFuturo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebimentoFuturoRel ? "default" : "secondary"}>
                    {item.recebimentoFuturoRel ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idTerceiro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.arquivoDocumento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.arquivoComprovante}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idClientePagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProcessoPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idArea}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.identificacaoPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.identificacaoPagar2}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.arquivoDocumento2}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.arquivoComprovante2}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.verba ? "default" : "secondary"}>
                    {item.verba ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.verbaDataDe}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.verbaDataAte}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.verbaEstado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.verbaCidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCentroResultado}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.secundaria ? "default" : "secondary"}>
                    {item.secundaria ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.geradoPeloProcesso ? "default" : "secondary"}>
                    {item.geradoPeloProcesso ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sequenciaHerdeiro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idUnidade}
                  
                </TableCell>
                
                <TableCell className="text-right">
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" size="icon">
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <DropdownMenuLabel>Actions</DropdownMenuLabel>
                      <DropdownMenuSeparator />
                      <DropdownMenuItem onClick={() => onEdit(item)}>
                        <Pencil className="mr-2 h-4 w-4" />
                        Edit
                      </DropdownMenuItem>
                      <DropdownMenuItem 
                        className="text-destructive"
                        onClick={() => handleDelete(item.id)}
                      >
                        <Trash2 className="mr-2 h-4 w-4" />
                        Delete
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
            {(!data?.items || data.items.length === 0) && (
              <TableRow>
                <TableCell colSpan={99} className="h-24 text-center">
                  No results found.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}
