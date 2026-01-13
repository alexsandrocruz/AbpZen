import { useMemo, useState } from "react";
import { useFinPlanoContases, useDeleteFinPlanoContas } from "@/lib/abp/hooks/useFinPlanoContases";
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

interface FinPlanoContasListProps {
  onEdit: (item: any) => void;
}

export function FinPlanoContasList({ onEdit }: FinPlanoContasListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useFinPlanoContases({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFinPlanoContas();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this finplanocontas?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("finPlanoContas deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete finplanocontas");
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
              placeholder="Search finplanocontases..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idPlanoConta</TableHead>
              
              <TableHead>idGrupo</TableHead>
              
              <TableHead>titulo</TableHead>
              
              <TableHead>codigo</TableHead>
              
              <TableHead>pagamentoSempreLiberado</TableHead>
              
              <TableHead>permiteLancamentoQuitado</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>padraoVendas</TableHead>
              
              <TableHead>antecipaVencimento</TableHead>
              
              <TableHead>terceiroNivel</TableHead>
              
              <TableHead>criarPeloFinanceiro</TableHead>
              
              <TableHead>valoresRestritos</TableHead>
              
              <TableHead>naoAbatePagtoDoSaldoDoCliente</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idPlanoConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idGrupo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.titulo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.codigo}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.pagamentoSempreLiberado ? "default" : "secondary"}>
                    {item.pagamentoSempreLiberado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.permiteLancamentoQuitado ? "default" : "secondary"}>
                    {item.permiteLancamentoQuitado ? "Yes" : "No"}
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
                  
                  <Badge variant={item.padraoVendas ? "default" : "secondary"}>
                    {item.padraoVendas ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.antecipaVencimento ? "default" : "secondary"}>
                    {item.antecipaVencimento ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.terceiroNivel ? "default" : "secondary"}>
                    {item.terceiroNivel ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.criarPeloFinanceiro ? "default" : "secondary"}>
                    {item.criarPeloFinanceiro ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.valoresRestritos ? "default" : "secondary"}>
                    {item.valoresRestritos ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.naoAbatePagtoDoSaldoDoCliente ? "default" : "secondary"}>
                    {item.naoAbatePagtoDoSaldoDoCliente ? "Yes" : "No"}
                  </Badge>
                  
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
