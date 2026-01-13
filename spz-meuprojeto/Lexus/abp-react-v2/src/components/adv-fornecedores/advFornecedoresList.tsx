import { useMemo, useState } from "react";
import { useAdvFornecedoreses, useDeleteAdvFornecedores } from "@/lib/abp/hooks/useAdvFornecedoreses";
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

interface AdvFornecedoresListProps {
  onEdit: (item: any) => void;
}

export function AdvFornecedoresList({ onEdit }: AdvFornecedoresListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvFornecedoreses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvFornecedores();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advfornecedores?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advFornecedores deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advfornecedores");
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
              placeholder="Search advfornecedoreses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idFornecedor</TableHead>
              
              <TableHead>apelido</TableHead>
              
              <TableHead>nome</TableHead>
              
              <TableHead>email</TableHead>
              
              <TableHead>telCelular</TableHead>
              
              <TableHead>telCelularObs</TableHead>
              
              <TableHead>telFixo</TableHead>
              
              <TableHead>telFixoObs</TableHead>
              
              <TableHead>endereco</TableHead>
              
              <TableHead>numero</TableHead>
              
              <TableHead>complemento</TableHead>
              
              <TableHead>bairro</TableHead>
              
              <TableHead>cep</TableHead>
              
              <TableHead>estado</TableHead>
              
              <TableHead>cidade</TableHead>
              
              <TableHead>observacoes</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>parceiroEmProcesso</TableHead>
              
              <TableHead>parceiroEmProcessoPerc</TableHead>
              
              <TableHead>idProfissional</TableHead>
              
              <TableHead>foto</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idFornecedor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.apelido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nome}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.email}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelular}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelularObs}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixoObs}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.endereco}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.numero}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.complemento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bairro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cep}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.estado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.observacoes}
                  
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
                  
                  <Badge variant={item.parceiroEmProcesso ? "default" : "secondary"}>
                    {item.parceiroEmProcesso ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.parceiroEmProcessoPerc}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProfissional}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.foto}
                  
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
