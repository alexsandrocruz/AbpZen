import { useMemo, useState } from "react";
import { useFdtDevses, useDeleteFdtDevs } from "@/lib/abp/hooks/useFdtDevses";
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

interface FdtDevsListProps {
  onEdit: (item: any) => void;
}

export function FdtDevsList({ onEdit }: FdtDevsListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useFdtDevses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFdtDevs();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this fdtdevs?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("fdtDevs deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete fdtdevs");
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
              placeholder="Search fdtdevses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idDev</TableHead>
              
              <TableHead>pacote</TableHead>
              
              <TableHead>descricao</TableHead>
              
              <TableHead>pendente</TableHead>
              
              <TableHead>aprovado</TableHead>
              
              <TableHead>reprovado</TableHead>
              
              <TableHead>finalizado</TableHead>
              
              <TableHead>comentariosRevisor</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>incluidoPor</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>alteradoPor</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idDev}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.pacote}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.descricao}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.pendente ? "default" : "secondary"}>
                    {item.pendente ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.aprovado ? "default" : "secondary"}>
                    {item.aprovado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.reprovado ? "default" : "secondary"}>
                    {item.reprovado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.finalizado ? "default" : "secondary"}>
                    {item.finalizado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.comentariosRevisor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsInclusao ? new Date(item.tsInclusao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.incluidoPor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsAlteracao ? new Date(item.tsAlteracao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.alteradoPor}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.ativo ? "default" : "secondary"}>
                    {item.ativo ? "Yes" : "No"}
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
