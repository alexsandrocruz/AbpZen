import { useMemo, useState } from "react";
import { useOpoOportunidadeses, useDeleteOpoOportunidades } from "@/lib/abp/hooks/useOpoOportunidadeses";
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

interface OpoOportunidadesListProps {
  onEdit: (item: any) => void;
}

export function OpoOportunidadesList({ onEdit }: OpoOportunidadesListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useOpoOportunidadeses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteOpoOportunidades();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this opooportunidades?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("opoOportunidades deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete opooportunidades");
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
              placeholder="Search opooportunidadeses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idOportunidade</TableHead>
              
              <TableHead>idCliente</TableHead>
              
              <TableHead>idUsuario</TableHead>
              
              <TableHead>idTipo</TableHead>
              
              <TableHead>idSituacao</TableHead>
              
              <TableHead>titulo</TableHead>
              
              <TableHead>numero</TableHead>
              
              <TableHead>dataInicio</TableHead>
              
              <TableHead>dataEstimada</TableHead>
              
              <TableHead>valorEstimado</TableHead>
              
              <TableHead>comentario</TableHead>
              
              <TableHead>aproveitada</TableHead>
              
              <TableHead>aproveitadaData</TableHead>
              
              <TableHead>cancelada</TableHead>
              
              <TableHead>canceladaMotivo</TableHead>
              
              <TableHead>canceladaData</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>indicadorCanceladoVisto</TableHead>
              
              <TableHead>valorEstimadoMensal</TableHead>
              
              <TableHead>deProcesso</TableHead>
              
              <TableHead>aproveitadaMotivo</TableHead>
              
              <TableHead>numeroProcesso</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idOportunidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idUsuario}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idSituacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.titulo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.numero}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataInicio}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataEstimada}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorEstimado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.comentario}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.aproveitada ? "default" : "secondary"}>
                    {item.aproveitada ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.aproveitadaData}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.cancelada ? "default" : "secondary"}>
                    {item.cancelada ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.canceladaMotivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.canceladaData}
                  
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
                  
                  <Badge variant={item.indicadorCanceladoVisto ? "default" : "secondary"}>
                    {item.indicadorCanceladoVisto ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorEstimadoMensal}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.deProcesso ? "default" : "secondary"}>
                    {item.deProcesso ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.aproveitadaMotivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.numeroProcesso}
                  
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
