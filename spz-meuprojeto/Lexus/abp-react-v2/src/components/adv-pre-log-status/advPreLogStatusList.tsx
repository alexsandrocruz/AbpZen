import { useMemo, useState } from "react";
import { useAdvPreLogStatuses, useDeleteAdvPreLogStatus } from "@/lib/abp/hooks/useAdvPreLogStatuses";
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

interface AdvPreLogStatusListProps {
  onEdit: (item: any) => void;
}

export function AdvPreLogStatusList({ onEdit }: AdvPreLogStatusListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvPreLogStatuses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvPreLogStatus();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advprelogstatus?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advPreLogStatus deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advprelogstatus");
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
              placeholder="Search advprelogstatuses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idLog</TableHead>
              
              <TableHead>idProcesso</TableHead>
              
              <TableHead>idStatus</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>conversao</TableHead>
              
              <TableHead>tsConversao</TableHead>
              
              <TableHead>perdido</TableHead>
              
              <TableHead>tsPerdido</TableHead>
              
              <TableHead>diasCorridosDoAnterior</TableHead>
              
              <TableHead>usuario</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idLog}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idStatus}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsInclusao ? new Date(item.tsInclusao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.conversao ? "default" : "secondary"}>
                    {item.conversao ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsConversao ? new Date(item.tsConversao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.perdido ? "default" : "secondary"}>
                    {item.perdido ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsPerdido ? new Date(item.tsPerdido).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.diasCorridosDoAnterior}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.usuario}
                  
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
