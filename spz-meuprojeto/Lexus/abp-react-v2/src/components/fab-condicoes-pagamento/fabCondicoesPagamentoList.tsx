import { useMemo, useState } from "react";
import { useFabCondicoesPagamentos, useDeleteFabCondicoesPagamento } from "@/lib/abp/hooks/useFabCondicoesPagamentos";
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

interface FabCondicoesPagamentoListProps {
  onEdit: (item: any) => void;
}

export function FabCondicoesPagamentoList({ onEdit }: FabCondicoesPagamentoListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useFabCondicoesPagamentos({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFabCondicoesPagamento();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this fabcondicoespagamento?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("fabCondicoesPagamento deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete fabcondicoespagamento");
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
              placeholder="Search fabcondicoespagamentos..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idCondicaoPagamento</TableHead>
              
              <TableHead>titulo</TableHead>
              
              <TableHead>parcelas</TableHead>
              
              <TableHead>p1</TableHead>
              
              <TableHead>d1</TableHead>
              
              <TableHead>p2</TableHead>
              
              <TableHead>d2</TableHead>
              
              <TableHead>p3</TableHead>
              
              <TableHead>d3</TableHead>
              
              <TableHead>p4</TableHead>
              
              <TableHead>d4</TableHead>
              
              <TableHead>p5</TableHead>
              
              <TableHead>d5</TableHead>
              
              <TableHead>p6</TableHead>
              
              <TableHead>d6</TableHead>
              
              <TableHead>p7</TableHead>
              
              <TableHead>d7</TableHead>
              
              <TableHead>p8</TableHead>
              
              <TableHead>d8</TableHead>
              
              <TableHead>p9</TableHead>
              
              <TableHead>d9</TableHead>
              
              <TableHead>p10</TableHead>
              
              <TableHead>d10</TableHead>
              
              <TableHead>p11</TableHead>
              
              <TableHead>d11</TableHead>
              
              <TableHead>p12</TableHead>
              
              <TableHead>d12</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>compras</TableHead>
              
              <TableHead>vendas</TableHead>
              
              <TableHead>valorMinimo</TableHead>
              
              <TableHead>atendimento</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idCondicaoPagamento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.titulo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.parcelas}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p1}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d1}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p2}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d2}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p3}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d3}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p4}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d4}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p5}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d5}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p6}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d6}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p7}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d7}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p8}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d8}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p9}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d9}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p10}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d10}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p11}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d11}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.p12}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.d12}
                  
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
                  
                  <Badge variant={item.compras ? "default" : "secondary"}>
                    {item.compras ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.vendas ? "default" : "secondary"}>
                    {item.vendas ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorMinimo}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.atendimento ? "default" : "secondary"}>
                    {item.atendimento ? "Yes" : "No"}
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
