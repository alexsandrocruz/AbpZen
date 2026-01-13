import { useMemo, useState } from "react";
import { useAdvClientesINSSes, useDeleteAdvClientesINSS } from "@/lib/abp/hooks/useAdvClientesINSSes";
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

interface AdvClientesINSSListProps {
  onEdit: (item: any) => void;
}

export function AdvClientesINSSList({ onEdit }: AdvClientesINSSListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvClientesINSSes({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvClientesINSS();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advclientesinss?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advClientesINSS deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advclientesinss");
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
              placeholder="Search advclientesinsses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idInssAgendado</TableHead>
              
              <TableHead>idCliente</TableHead>
              
              <TableHead>inssAgendado</TableHead>
              
              <TableHead>inssData</TableHead>
              
              <TableHead>inssIdTipoBeneficio</TableHead>
              
              <TableHead>inssIdPosto</TableHead>
              
              <TableHead>inssResultado</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>inssResultadoIndicadorOculto</TableHead>
              
              <TableHead>inssResponsavel</TableHead>
              
              <TableHead>inssProtocolo</TableHead>
              
              <TableHead>inssIdUsuarioInclusao</TableHead>
              
              <TableHead>idStatus</TableHead>
              
              <TableHead>dataFinalizacao</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idInssAgendado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.inssAgendado ? "default" : "secondary"}>
                    {item.inssAgendado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssData}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssIdTipoBeneficio}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssIdPosto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssResultado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsInclusao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsAlteracao}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.inssResultadoIndicadorOculto ? "default" : "secondary"}>
                    {item.inssResultadoIndicadorOculto ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssResponsavel}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssProtocolo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssIdUsuarioInclusao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idStatus}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataFinalizacao}
                  
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
