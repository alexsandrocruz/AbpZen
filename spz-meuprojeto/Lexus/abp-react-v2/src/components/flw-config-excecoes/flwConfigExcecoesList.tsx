import { useMemo, useState } from "react";
import { useFlwConfigExcecoeses, useDeleteFlwConfigExcecoes } from "@/lib/abp/hooks/useFlwConfigExcecoeses";
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

interface FlwConfigExcecoesListProps {
  onEdit: (item: any) => void;
}

export function FlwConfigExcecoesList({ onEdit }: FlwConfigExcecoesListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useFlwConfigExcecoeses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFlwConfigExcecoes();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this flwconfigexcecoes?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("flwConfigExcecoes deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete flwconfigexcecoes");
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
              placeholder="Search flwconfigexcecoeses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idConfig</TableHead>
              
              <TableHead>tipoMarcacoes</TableHead>
              
              <TableHead>idHistoricoTipo</TableHead>
              
              <TableHead>data</TableHead>
              
              <TableHead>qtde</TableHead>
              
              <TableHead>manhaQtde</TableHead>
              
              <TableHead>tardeQtde</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idConfig}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tipoMarcacoes}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idHistoricoTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.data}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.qtde}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.manhaQtde}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tardeQtde}
                  
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
