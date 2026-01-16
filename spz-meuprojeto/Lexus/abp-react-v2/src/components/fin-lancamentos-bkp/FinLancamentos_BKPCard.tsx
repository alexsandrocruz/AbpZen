import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { MoreHorizontal, Pencil, Trash2, Box } from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

interface FinLancamentos_BKPCardProps {
  item: any;
  onEdit: (item: any) => void;
  onDelete: (id: string) => void;
}

export function FinLancamentos_BKPCard({ item, onEdit, onDelete }: FinLancamentos_BKPCardProps) {
  return (
    <Card className="flex flex-col h-full hover:shadow-md transition-shadow">
      <CardHeader className="flex flex-row items-start justify-between space-y-0 pb-2">
        <div className="flex items-center gap-2">
          
          
          <Badge variant={item.quitado ? "default" : "secondary"}>
            {item.quitado ? "quitado" : "Not quitado"}
          </Badge>
          
          <Badge variant={item.recorrente ? "default" : "secondary"}>
            {item.recorrente ? "recorrente" : "Not recorrente"}
          </Badge>
          
        </div>
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="icon" className="h-8 w-8">
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
              onClick={() => onDelete(item.id)}
            >
              <Trash2 className="mr-2 h-4 w-4" />
              Delete
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </CardHeader>
      <CardContent className="flex-1 pt-0">
        <div className="flex items-start gap-3">
          <div className="bg-primary/10 p-2 rounded-lg shrink-0">
            <Box className="h-5 w-5 text-primary" />
          </div>
          <div className="min-w-0 flex-1">
            
            
            
            <h3 className="font-semibold truncate" title={item.operacao}>
              {item.operacao || "-"}
            </h3>
            
            
            
            <p className="text-sm text-muted-foreground truncate" title={item.modulo}>
              {item.modulo || "-"}
            </p>
            
          </div>
        </div>
      </CardContent>
      <CardFooter className="pt-0">
        <Button 
          variant="outline" 
          size="sm" 
          className="w-full"
          onClick={() => onEdit(item)}
        >
          <Pencil className="mr-2 h-3 w-3" />
          Edit
        </Button>
      </CardFooter>
    </Card>
  );
}
