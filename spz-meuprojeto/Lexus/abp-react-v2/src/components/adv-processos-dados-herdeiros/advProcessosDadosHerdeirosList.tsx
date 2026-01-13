import { useMemo, useState } from "react";
import { useAdvProcessosDadosHerdeiroses, useDeleteAdvProcessosDadosHerdeiros } from "@/lib/abp/hooks/useAdvProcessosDadosHerdeiroses";
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

interface AdvProcessosDadosHerdeirosListProps {
  onEdit: (item: any) => void;
}

export function AdvProcessosDadosHerdeirosList({ onEdit }: AdvProcessosDadosHerdeirosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvProcessosDadosHerdeiroses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvProcessosDadosHerdeiros();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advprocessosdadosherdeiros?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advProcessosDadosHerdeiros deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advprocessosdadosherdeiros");
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
              placeholder="Search advprocessosdadosherdeiroses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idHerdeiro</TableHead>
              
              <TableHead>idProcesso</TableHead>
              
              <TableHead>sequencia</TableHead>
              
              <TableHead>bancarioBancoId</TableHead>
              
              <TableHead>bancarioTipoConta</TableHead>
              
              <TableHead>bancarioAgencia</TableHead>
              
              <TableHead>bancarioConta</TableHead>
              
              <TableHead>bancarioFavorecido</TableHead>
              
              <TableHead>bancarioCpf</TableHead>
              
              <TableHead>bancarioPerc</TableHead>
              
              <TableHead>bancarioTarifa</TableHead>
              
              <TableHead>bancarioTarifaParcelas</TableHead>
              
              <TableHead>idHonorario</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idHerdeiro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sequencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioBancoId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioTipoConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioAgencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioFavorecido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioCpf}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioPerc}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioTarifa}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioTarifaParcelas}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idHonorario}
                  
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
