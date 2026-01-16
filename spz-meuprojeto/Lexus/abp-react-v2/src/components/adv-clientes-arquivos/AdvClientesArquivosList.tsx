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
import { Search, MoreHorizontal, Pencil, Trash2, Loader2, LayoutGrid, List } from "lucide-react";
import { toast } from "sonner";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { AdvClientesArquivosCard } from "./AdvClientesArquivosCard";

interface AdvClientesArquivosListProps {
  onEdit: (item: any) => void;
}

export function AdvClientesArquivosList({ onEdit }: AdvClientesArquivosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
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
        <div className="p-4 border-b flex items-center justify-between gap-4">
          <div className="relative max-w-sm flex-1">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search advclientesarquivoses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-1 border rounded-md p-1 bg-muted/20">
            <Button 
              variant={viewMode === "grid" ? "secondary" : "ghost"} 
              size="icon"
              className="h-8 w-8"
              onClick={() => setViewMode("grid")}
            >
              <LayoutGrid className="h-4 w-4" />
            </Button>
            <Button 
              variant={viewMode === "list" ? "secondary" : "ghost"} 
              size="icon"
              className="h-8 w-8"
              onClick={() => setViewMode("list")}
            >
              <List className="h-4 w-4" />
            </Button>
          </div>
        </div>

        {viewMode === "list" ? (
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
        ) : (
          <div className="p-4">
            {(!data?.items || data.items.length === 0) ? (
              <div className="flex items-center justify-center h-24 text-muted-foreground">
                No results found.
              </div>
            ) : (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                {data.items.map((item: any) => (
                  <AdvClientesArquivosCard
                    key={item.id}
                    item={item}
                    onEdit={onEdit}
                    onDelete={handleDelete}
                  />
                ))}
              </div>
            )}
          </div>
        )}
      </CardContent>
    </Card>
  );
}
