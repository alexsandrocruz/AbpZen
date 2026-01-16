import { useMemo, useState } from "react";
import { useAdvVerbases, useDeleteAdvVerbas } from "@/lib/abp/hooks/useAdvVerbases";
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
import { AdvVerbasCard } from "./AdvVerbasCard";

interface AdvVerbasListProps {
  onEdit: (item: any) => void;
}

export function AdvVerbasList({ onEdit }: AdvVerbasListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const { data, isLoading, isError } = useAdvVerbases({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvVerbas();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advverbas?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advVerbas deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advverbas");
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
              placeholder="Search advverbases..."
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
                
                <TableHead>idVerba</TableHead>
                
                <TableHead>idTipo</TableHead>
                
                <TableHead>idProfissional</TableHead>
                
                <TableHead>idProcesso</TableHead>
                
                <TableHead>idLancamento</TableHead>
                
                <TableHead>valor</TableHead>
                
                <TableHead>dataDe</TableHead>
                
                <TableHead>dataAte</TableHead>
                
                <TableHead>estado</TableHead>
                
                <TableHead>cidade</TableHead>
                
                <TableHead>comprovante</TableHead>
                
                <TableHead>comprovanteArquivo</TableHead>
                
                <TableHead>solicitacao</TableHead>
                
                <TableHead>aceito</TableHead>
                
                <TableHead>ativo</TableHead>
                
                <TableHead>tsInclusao</TableHead>
                
                <TableHead>tsAlteracao</TableHead>
                
                <TableHead>incluidoPor</TableHead>
                
                <TableHead>alteradoPor</TableHead>
                
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items?.map((item: any) => (
                <TableRow key={item.id}>
                  
                  <TableCell>
                    
                    {item.idVerba}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idTipo}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idProfissional}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idProcesso}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idLancamento}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.valor}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.dataDe}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.dataAte}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.estado}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.cidade}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.comprovante ? "default" : "secondary"}>
                      {item.comprovante ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.comprovanteArquivo}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.solicitacao ? "default" : "secondary"}>
                      {item.solicitacao ? "Yes" : "No"}
                    </Badge>
                    
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
                    
                    {item.incluidoPor}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.alteradoPor}
                    
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
                  <AdvVerbasCard
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
