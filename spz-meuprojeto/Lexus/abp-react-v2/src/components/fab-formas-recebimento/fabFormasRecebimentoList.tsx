import { useMemo, useState } from "react";
import { useFabFormasRecebimentos, useDeleteFabFormasRecebimento } from "@/lib/abp/hooks/useFabFormasRecebimentos";
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

interface FabFormasRecebimentoListProps {
  onEdit: (item: any) => void;
}

export function FabFormasRecebimentoList({ onEdit }: FabFormasRecebimentoListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useFabFormasRecebimentos({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFabFormasRecebimento();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this fabformasrecebimento?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("fabFormasRecebimento deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete fabformasrecebimento");
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
              placeholder="Search fabformasrecebimentos..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idFormaRecebimento</TableHead>
              
              <TableHead>titulo</TableHead>
              
              <TableHead>ordem</TableHead>
              
              <TableHead>padrao</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>idCondicaoPagamento</TableHead>
              
              <TableHead>online</TableHead>
              
              <TableHead>tipo</TableHead>
              
              <TableHead>emailPagSeguro</TableHead>
              
              <TableHead>texto</TableHead>
              
              <TableHead>contasReceber</TableHead>
              
              <TableHead>vendas</TableHead>
              
              <TableHead>diasParaPrevisao</TableHead>
              
              <TableHead>valorDesconto</TableHead>
              
              <TableHead>descontoTipo</TableHead>
              
              <TableHead>recebimentoFuturo</TableHead>
              
              <TableHead>recebimentoFuturoDias</TableHead>
              
              <TableHead>recebimentoFuturoTaxa</TableHead>
              
              <TableHead>idConta</TableHead>
              
              <TableHead>idPlanoConta</TableHead>
              
              <TableHead>idCentroCusto</TableHead>
              
              <TableHead>idContaPagar</TableHead>
              
              <TableHead>idPlanoContaPagar</TableHead>
              
              <TableHead>idCentroCustoPagar</TableHead>
              
              <TableHead>idFormaPagar</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idFormaRecebimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.titulo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.ordem}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.padrao ? "default" : "secondary"}>
                    {item.padrao ? "Yes" : "No"}
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
                  
                  {item.idCondicaoPagamento}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.online ? "default" : "secondary"}>
                    {item.online ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.emailPagSeguro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.texto}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.contasReceber ? "default" : "secondary"}>
                    {item.contasReceber ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.vendas ? "default" : "secondary"}>
                    {item.vendas ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.diasParaPrevisao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorDesconto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descontoTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebimentoFuturo ? "default" : "secondary"}>
                    {item.recebimentoFuturo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recebimentoFuturoDias}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recebimentoFuturoTaxa}
                  
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
                  
                  {item.idContaPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idPlanoContaPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCentroCustoPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idFormaPagar}
                  
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
