import { Shell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Plus, Box, Search, MoreHorizontal, Pencil, Trash2, Loader2 } from "lucide-react";
import { useState } from "react";
import { useLocation } from "wouter";
import { usePropostalItems, useDeletePropostalItem } from "@/lib/abp/hooks/usePropostalItems";
import { toast } from "sonner";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

export default function PropostalItemsPage() {
  const [, setLocation] = useLocation();
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading } = usePropostalItems({ filter: searchTerm });
  const deleteMutation = useDeletePropostalItem();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this propostalitem?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("PropostalItem deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete");
      }
    }
  };

  return (
    <Shell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="bg-primary/10 p-2 rounded-lg">
              <Box className="h-6 w-6 text-primary" />
            </div>
            <div>
              <h1 className="text-3xl font-bold tracking-tight">PropostalItems</h1>
              <p className="text-muted-foreground">Manage your propostalitems</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/propostal-item/create")}>
            <Plus className="size-4" />
            New PropostalItem
          </Button>
        </div>

        <Card>
          <CardContent className="p-0">
            <div className="p-4 border-b">
              <div className="relative max-w-sm">
                <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Search propostalitems..."
                  className="pl-8"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                />
              </div>
            </div>

            {isLoading ? (
              <div className="flex items-center justify-center p-12">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    
                    <TableHead>Desc</TableHead>
                    
                    <TableHead>Quant</TableHead>
                    
                    <TableHead>UnitPrice</TableHead>
                    
                    <TableHead>Total</TableHead>
                    
                    <TableHead>ProposalId</TableHead>
                    
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {data?.items?.map((item: any) => (
                    <TableRow 
                      key={item.id} 
                      className="cursor-pointer hover:bg-muted/50"
                      onClick={() => setLocation(`/admin/propostal-item/${item.id}/edit`)}
                    >
                      
                      <TableCell>
                        
                        {item.desc}
                        
                      </TableCell>
                      
                      <TableCell>
                        
                        {item.quant}
                        
                      </TableCell>
                      
                      <TableCell>
                        
                        {item.unitPrice}
                        
                      </TableCell>
                      
                      <TableCell>
                        
                        {item.total}
                        
                      </TableCell>
                      
                      <TableCell>
                        
                        {item.proposalId}
                        
                      </TableCell>
                      
                      <TableCell className="text-right" onClick={(e) => e.stopPropagation()}>
                        <DropdownMenu>
                          <DropdownMenuTrigger asChild>
                            <Button variant="ghost" size="icon">
                              <MoreHorizontal className="h-4 w-4" />
                            </Button>
                          </DropdownMenuTrigger>
                          <DropdownMenuContent align="end">
                            <DropdownMenuLabel>Actions</DropdownMenuLabel>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem onClick={() => setLocation(`/admin/propostal-item/${item.id}/edit`)}>
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
            )}
          </CardContent>
        </Card>
      </div>
    </Shell>
  );
}
