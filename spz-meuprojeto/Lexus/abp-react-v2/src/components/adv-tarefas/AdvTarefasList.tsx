import { useMemo, useState } from "react";
import { useAdvTarefases, useDeleteAdvTarefas } from "@/lib/abp/hooks/useAdvTarefases";
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
import { AdvTarefasCard } from "./AdvTarefasCard";

interface AdvTarefasListProps {
  onEdit: (item: any) => void;
}

export function AdvTarefasList({ onEdit }: AdvTarefasListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const { data, isLoading, isError } = useAdvTarefases({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvTarefas();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advtarefas?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advTarefas deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advtarefas");
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
              placeholder="Search advtarefases..."
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
                
                <TableHead>idTarefa</TableHead>
                
                <TableHead>idTipoTarefa</TableHead>
                
                <TableHead>idCompromisso</TableHead>
                
                <TableHead>idProcesso</TableHead>
                
                <TableHead>dataCadastro</TableHead>
                
                <TableHead>dataParaFinalizacao</TableHead>
                
                <TableHead>descricao</TableHead>
                
                <TableHead>ResponsibleUserId</TableHead>
                
                <TableHead>idExecutor</TableHead>
                
                <TableHead>finalizado</TableHead>
                
                <TableHead>tsFinalizacao</TableHead>
                
                <TableHead>ativo</TableHead>
                
                <TableHead>tsInclusao</TableHead>
                
                <TableHead>tsAlteracao</TableHead>
                
                <TableHead>incluidoPor</TableHead>
                
                <TableHead>alteradoPor</TableHead>
                
                <TableHead>agendada</TableHead>
                
                <TableHead>horarioInicial</TableHead>
                
                <TableHead>horarioFinal</TableHead>
                
                <TableHead>onde</TableHead>
                
                <TableHead>idCliente</TableHead>
                
                <TableHead>idUsuarioFinalizou</TableHead>
                
                <TableHead>lembreteQuandoFinalizarPara</TableHead>
                
                <TableHead>tecnica</TableHead>
                
                <TableHead>coletivoOriginal</TableHead>
                
                <TableHead>coletivoIdOriginal</TableHead>
                
                <TableHead>coletivoIdCliente</TableHead>
                
                <TableHead>pauta</TableHead>
                
                <TableHead>pautaIdUsuarioResp</TableHead>
                
                <TableHead>pautaRespAceite</TableHead>
                
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items?.map((item: any) => (
                <TableRow key={item.id}>
                  
                  <TableCell>
                    
                    {item.idTarefa}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idTipoTarefa}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idCompromisso}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idProcesso}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.dataCadastro}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.dataParaFinalizacao}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.descricao}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.responsibleUserId}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idExecutor}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.finalizado ? "default" : "secondary"}>
                      {item.finalizado ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.tsFinalizacao ? new Date(item.tsFinalizacao).toLocaleDateString() : "-"}
                    
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
                  
                  <TableCell>
                    
                    <Badge variant={item.agendada ? "default" : "secondary"}>
                      {item.agendada ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.horarioInicial}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.horarioFinal}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.onde}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idCliente}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idUsuarioFinalizou}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.lembreteQuandoFinalizarPara}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.tecnica ? "default" : "secondary"}>
                      {item.tecnica ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.coletivoOriginal ? "default" : "secondary"}>
                      {item.coletivoOriginal ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.coletivoIdOriginal}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.coletivoIdCliente}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.pauta ? "default" : "secondary"}>
                      {item.pauta ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.pautaIdUsuarioResp}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.pautaRespAceite ? "default" : "secondary"}>
                      {item.pautaRespAceite ? "Yes" : "No"}
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
                  <AdvTarefasCard
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
