import { useMemo, useState } from "react";
import { useAdvClientesArquivoses, useDeleteAdvClientesArquivos } from "@/lib/abp/hooks/useAdvClientesArquivoses";
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

interface AdvClientesArquivosListProps {
  onEdit: (item: any) => void;
}

export function AdvClientesArquivosList({ onEdit }: AdvClientesArquivosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvClientesArquivoses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvClientesArquivos();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advclientesarquivos?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advClientesArquivos deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advclientesarquivos");
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
              placeholder="Search advclientesarquivoses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idArquivo</TableHead>
              
              <TableHead>idCliente</TableHead>
              
              <TableHead>idTipoArquivo</TableHead>
              
              <TableHead>descricao</TableHead>
              
              <TableHead>arquivo</TableHead>
              
              <TableHead>incluidoPor</TableHead>
              
              <TableHead>alteradoPor</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>idProcesso</TableHead>
              
              <TableHead>precisaRevisao</TableHead>
              
              <TableHead>idSolicitante</TableHead>
              
              <TableHead>solicitanteComentario</TableHead>
              
              <TableHead>idRevisor</TableHead>
              
              <TableHead>revisorComentario</TableHead>
              
              <TableHead>reprovado</TableHead>
              
              <TableHead>pendenteVisualizacaoAprovacao</TableHead>
              
              <TableHead>status</TableHead>
              
              <TableHead>autoFTP</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idArquivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idTipoArquivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descricao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.arquivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.incluidoPor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.alteradoPor}
                  
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
                  
                  {item.idProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.precisaRevisao ? "default" : "secondary"}>
                    {item.precisaRevisao ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idSolicitante}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.solicitanteComentario}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idRevisor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.revisorComentario}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.reprovado}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.pendenteVisualizacaoAprovacao ? "default" : "secondary"}>
                    {item.pendenteVisualizacaoAprovacao ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.status}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.autoFTP ? "default" : "secondary"}>
                    {item.autoFTP ? "Yes" : "No"}
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
