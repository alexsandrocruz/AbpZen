import { useMemo, useState } from "react";
import { useClientContacts, useDeleteClientContact } from "@/lib/abp/hooks/useClientContacts";
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

interface ClientContactListProps {
  onEdit: (item: any) => void;
}

export function ClientContactList({ onEdit }: ClientContactListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useClientContacts({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteClientContact();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this clientcontact?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("ClientContact deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete clientcontact");
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
              placeholder="Search clientcontacts..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>Name</TableHead>
              
              <TableHead>Role</TableHead>
              
              <TableHead>Email</TableHead>
              
              <TableHead>Phone</TableHead>
              
              <TableHead>IsPrimary</TableHead>
              
              <TableHead>ClientId</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.name}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.role}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.email}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.phone}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.isPrimary ? "default" : "secondary"}>
                    {item.isPrimary ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.clientId}
                  
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
