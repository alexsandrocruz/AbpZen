import { useMemo, useState } from "react";
import { useEvents, useDeleteEvent } from "@/lib/abp/hooks/useEvents";
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

interface EventListProps {
  onEdit: (item: any) => void;
}

export function EventList({ onEdit }: EventListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useEvents({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteEvent();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this event?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("Event deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete event");
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
              placeholder="Search events..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>ArtistId</TableHead>
              
              <TableHead>ClientId</TableHead>
              
              <TableHead>LocalPartnerId</TableHead>
              
              <TableHead>LocationId</TableHead>
              
              <TableHead>Title</TableHead>
              
              <TableHead>Type</TableHead>
              
              <TableHead>Status</TableHead>
              
              <TableHead>StartDateTime</TableHead>
              
              <TableHead>EndDateTime</TableHead>
              
              <TableHead>Fee</TableHead>
              
              <TableHead>Description</TableHead>
              
              <TableHead>TaxPercentage</TableHead>
              
              <TableHead>TaxValue</TableHead>
              
              <TableHead>ContractType</TableHead>
              
              <TableHead>NegotiationType</TableHead>
              
              <TableHead>HasConflict</TableHead>
              
              <TableHead>SuggestedAlternativeArtistId</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.artistId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.clientId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.localPartnerId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.locationId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.title}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.type}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.status}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.startDateTime ? new Date(item.startDateTime).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.endDateTime ? new Date(item.endDateTime).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.fee}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.description}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.taxPercentage}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.taxValue}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.contractType}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.negotiationType}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.hasConflict ? "default" : "secondary"}>
                    {item.hasConflict ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.suggestedAlternativeArtistId}
                  
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
