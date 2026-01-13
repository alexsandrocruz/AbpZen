import { useMemo, useState } from "react";
import { useAdvCompromissoses, useDeleteAdvCompromissos } from "@/lib/abp/hooks/useAdvCompromissoses";
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

interface AdvCompromissosListProps {
  onEdit: (item: any) => void;
}

export function AdvCompromissosList({ onEdit }: AdvCompromissosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvCompromissoses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvCompromissos();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advcompromissos?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advCompromissos deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advcompromissos");
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
              placeholder="Search advcompromissoses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idCompromisso</TableHead>
              
              <TableHead>idTipoCompromisso</TableHead>
              
              <TableHead>idProcesso</TableHead>
              
              <TableHead>dataPublicacao</TableHead>
              
              <TableHead>dataPrazoInterno</TableHead>
              
              <TableHead>dataPrazoFatal</TableHead>
              
              <TableHead>descricao</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>incluidoPor</TableHead>
              
              <TableHead>alteradoPor</TableHead>
              
              <TableHead>idAgendamentoINSS</TableHead>
              
              <TableHead>pauta</TableHead>
              
              <TableHead>pautaIdUsuarioResp</TableHead>
              
              <TableHead>pautaRespAceite</TableHead>
              
              <TableHead>horarioInicial</TableHead>
              
              <TableHead>horarioFinal</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idCompromisso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idTipoCompromisso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPublicacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrazoInterno}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrazoFatal}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descricao}
                  
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
                  
                  {item.idAgendamentoINSS}
                  
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
                
                <TableCell>
                  
                  {item.horarioInicial}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.horarioFinal}
                  
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
