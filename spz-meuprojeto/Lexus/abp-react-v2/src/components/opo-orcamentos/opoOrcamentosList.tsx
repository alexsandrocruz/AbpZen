import { useMemo, useState } from "react";
import { useOpoOrcamentoses, useDeleteOpoOrcamentos } from "@/lib/abp/hooks/useOpoOrcamentoses";
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

interface OpoOrcamentosListProps {
  onEdit: (item: any) => void;
}

export function OpoOrcamentosList({ onEdit }: OpoOrcamentosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useOpoOrcamentoses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteOpoOrcamentos();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this opoorcamentos?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("opoOrcamentos deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete opoorcamentos");
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
              placeholder="Search opoorcamentoses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idOrcamento</TableHead>
              
              <TableHead>idOportunidade</TableHead>
              
              <TableHead>titulo</TableHead>
              
              <TableHead>dataCriacao</TableHead>
              
              <TableHead>valor</TableHead>
              
              <TableHead>arquivo</TableHead>
              
              <TableHead>aceito</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>dataValidade</TableHead>
              
              <TableHead>valorMensal</TableHead>
              
              <TableHead>comArquivo</TableHead>
              
              <TableHead>comProduto</TableHead>
              
              <TableHead>comProdutoTerceiro</TableHead>
              
              <TableHead>comServico</TableHead>
              
              <TableHead>valorDesconto</TableHead>
              
              <TableHead>valorAcrescimo</TableHead>
              
              <TableHead>valorFrete</TableHead>
              
              <TableHead>informacoes</TableHead>
              
              <TableHead>descontoPercentual</TableHead>
              
              <TableHead>valorItens</TableHead>
              
              <TableHead>idCondicaoPagamento</TableHead>
              
              <TableHead>dataPrevistaEntrega</TableHead>
              
              <TableHead>moeda</TableHead>
              
              <TableHead>valorConversao</TableHead>
              
              <TableHead>imprimeMoedaAdd</TableHead>
              
              <TableHead>valorDescontoMensal</TableHead>
              
              <TableHead>valorAcrescimoMensal</TableHead>
              
              <TableHead>valorFreteMensal</TableHead>
              
              <TableHead>descontoPercentualMensal</TableHead>
              
              <TableHead>valorItensMensal</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idOrcamento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idOportunidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.titulo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataCriacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.arquivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.aceito ? "default" : "secondary"}>
                    {item.aceito ? "Yes" : "No"}
                  </Badge>
                  
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
                  
                  {item.dataValidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorMensal}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.comArquivo ? "default" : "secondary"}>
                    {item.comArquivo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.comProduto ? "default" : "secondary"}>
                    {item.comProduto ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.comProdutoTerceiro ? "default" : "secondary"}>
                    {item.comProdutoTerceiro ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.comServico ? "default" : "secondary"}>
                    {item.comServico ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorDesconto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorAcrescimo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorFrete}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.informacoes}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descontoPercentual}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorItens}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCondicaoPagamento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevistaEntrega}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.moeda}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorConversao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.imprimeMoedaAdd}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorDescontoMensal}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorAcrescimoMensal}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorFreteMensal}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descontoPercentualMensal}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorItensMensal}
                  
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
