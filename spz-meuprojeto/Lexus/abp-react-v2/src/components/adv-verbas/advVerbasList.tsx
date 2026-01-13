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

interface AdvVerbasListProps {
  onEdit: (item: any) => void;
}

export function AdvVerbasList({ onEdit }: AdvVerbasListProps) {
  const [searchTerm, setSearchTerm] = useState("");
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
        <div className="p-4 border-b">
          <div className="relative max-w-sm">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search advverbases..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
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
      </CardContent>
    </Card>
  );
}
