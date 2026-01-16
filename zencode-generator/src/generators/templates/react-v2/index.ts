/**
 * React V2 (Vite + Tailwind 4 + shadcn) Template Generators
 * Based on abp-react-v2 project structure
 */

// ============ PAGE TEMPLATE ============

export function getReactV2PageTemplate(): string {
  return `import { Shell } from "@/components/layout/shell";
import { {{ entity.name | pascalCase }}List } from "@/components/{{ entity.name | kebabCase }}/{{ entity.name | pascalCase }}List";
import { {{ entity.name | pascalCase }}Form } from "@/components/{{ entity.name | kebabCase }}/{{ entity.name | pascalCase }}Form";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useState } from "react";

export default function {{ entity.pluralName | pascalCase }}Page() {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedItem, setSelectedItem] = useState<any>(null);

  const handleCreate = () => {
    setSelectedItem(null);
    setIsFormOpen(true);
  };

  const handleEdit = (item: any) => {
    setSelectedItem(item);
    setIsFormOpen(true);
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
              <h1 className="text-3xl font-bold tracking-tight">{{ entity.pluralName }}</h1>
              <p className="text-muted-foreground">Manage your {{ entity.pluralName | downcase }}</p>
            </div>
          </div>
          <Button className="gap-2" onClick={handleCreate}>
            <Plus className="size-4" />
            New {{ entity.name }}
          </Button>
        </div>

        <{{ entity.name | pascalCase }}List onEdit={handleEdit} />

        <{{ entity.name | pascalCase }}Form
          isOpen={isFormOpen}
          onClose={() => setIsFormOpen(false)}
          initialValues={selectedItem}
        />
      </div>
    </Shell>
  );
}
`;
}

// ============ LIST COMPONENT TEMPLATE ============

export function getReactV2ListComponentTemplate(): string {
  return `import { useMemo, useState } from "react";
import { use{{ entity.pluralName | pascalCase }}, useDelete{{ entity.name | pascalCase }} } from "@/lib/abp/hooks/use{{ entity.pluralName | pascalCase }}";
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
import { Search, MoreHorizontal, Pencil, Trash2, Loader2, LayoutGrid, List } from "lucide-react";
import { toast } from "sonner";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { {{ entity.name | pascalCase }}Card } from "./{{ entity.name | pascalCase }}Card";

interface {{ entity.name | pascalCase }}ListProps {
  onEdit: (item: any) => void;
}

export function {{ entity.name | pascalCase }}List({ onEdit }: {{ entity.name | pascalCase }}ListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const { data, isLoading, isError } = use{{ entity.pluralName | pascalCase }}({
    filter: searchTerm,
  });
  const deleteMutation = useDelete{{ entity.name | pascalCase }}();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this {{ entity.name | downcase }}?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("{{ entity.name }} deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete {{ entity.name | downcase }}");
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
        <div className="p-4 border-b flex items-center justify-between gap-4">
          <div className="relative max-w-sm flex-1">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search {{ entity.pluralName | downcase }}..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-1 border rounded-md p-1 bg-muted/20">
            <Button 
              variant={viewMode === "grid" ? "secondary" : "ghost"} 
              size="icon"
              className="h-8 w-8"
              onClick={() => setViewMode("grid")}
            >
              <LayoutGrid className="h-4 w-4" />
            </Button>
            <Button 
              variant={viewMode === "list" ? "secondary" : "ghost"} 
              size="icon"
              className="h-8 w-8"
              onClick={() => setViewMode("list")}
            >
              <List className="h-4 w-4" />
            </Button>
          </div>
        </div>

        {viewMode === "list" ? (
          <Table>
            <TableHeader>
              <TableRow>
                {% for field in entity.fields %}
                <TableHead>{{ field.name }}</TableHead>
                {% endfor %}
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items?.map((item: any) => (
                <TableRow key={item.id}>
                  {% for field in entity.fields %}
                  <TableCell>
                    {% if field.type == "bool" %}
                    <Badge variant={item.{{ field.name | camelCase }} ? "default" : "secondary"}>
                      {item.{{ field.name | camelCase }} ? "Yes" : "No"}
                    </Badge>
                    {% elsif field.type == "datetime" %}
                    {item.{{ field.name | camelCase }} ? new Date(item.{{ field.name | camelCase }}).toLocaleDateString() : "-"}
                    {% else %}
                    {item.{{ field.name | camelCase }}}
                    {% endif %}
                  </TableCell>
                  {% endfor %}
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
        ) : (
          <div className="p-4">
            {(!data?.items || data.items.length === 0) ? (
              <div className="flex items-center justify-center h-24 text-muted-foreground">
                No results found.
              </div>
            ) : (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                {data.items.map((item: any) => (
                  <{{ entity.name | pascalCase }}Card
                    key={item.id}
                    item={item}
                    onEdit={onEdit}
                    onDelete={handleDelete}
                  />
                ))}
              </div>
            )}
          </div>
        )}
      </CardContent>
    </Card>
  );
}
`;
}

// ============ CARD COMPONENT TEMPLATE ============

export function getReactV2CardComponentTemplate(): string {
  return `import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card";
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

interface {{ entity.name | pascalCase }}CardProps {
  item: any;
  onEdit: (item: any) => void;
  onDelete: (id: string) => void;
}

export function {{ entity.name | pascalCase }}Card({ item, onEdit, onDelete }: {{ entity.name | pascalCase }}CardProps) {
  return (
    <Card className="flex flex-col h-full hover:shadow-md transition-shadow">
      <CardHeader className="flex flex-row items-start justify-between space-y-0 pb-2">
        <div className="flex items-center gap-2">
          {% assign boolFields = entity.fields | where: "type", "bool" %}
          {% for field in boolFields limit: 2 %}
          <Badge variant={item.{{ field.name | camelCase }} ? "default" : "secondary"}>
            {item.{{ field.name | camelCase }} ? "{{ field.name }}" : "Not {{ field.name }}"}
          </Badge>
          {% endfor %}
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
            {% assign stringFields = entity.fields | where: "type", "string" %}
            {% assign firstStringField = stringFields | first %}
            {% if firstStringField %}
            <h3 className="font-semibold truncate" title={item.{{ firstStringField.name | camelCase }}}>
              {item.{{ firstStringField.name | camelCase }} || "-"}
            </h3>
            {% else %}
            <h3 className="font-semibold truncate">
              {item.id}
            </h3>
            {% endif %}
            {% assign secondStringField = stringFields[1] %}
            {% if secondStringField %}
            <p className="text-sm text-muted-foreground truncate" title={item.{{ secondStringField.name | camelCase }}}>
              {item.{{ secondStringField.name | camelCase }} || "-"}
            </p>
            {% endif %}
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
`;
}

// ============ FORM COMPONENT TEMPLATE ============

export function getReactV2FormComponentTemplate(): string {
  return `import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreate{{ entity.name | pascalCase }}, useUpdate{{ entity.name | pascalCase }} } from "@/lib/abp/hooks/use{{ entity.pluralName | pascalCase }}";
import { toast } from "sonner";

const formSchema = z.object({
  {% for field in entity.fields %}
    {{ field.name | camelCase }}: z.any(),
  {% endfor %}
});

type FormValues = z.infer<typeof formSchema>;

interface {{ entity.name | pascalCase }}FormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function {{ entity.name | pascalCase }}Form({
  isOpen,
  onClose,
  initialValues,
}: {{ entity.name | pascalCase }}FormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreate{{ entity.name | pascalCase
}}();
const updateMutation = useUpdate{{ entity.name | pascalCase }}();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("{{ entity.name }} updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("{{ entity.name }} created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save {{ entity.name | downcase }}:", error);
    toast.error(error.message || "Failed to save {{ entity.name | downcase }}");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit {{ entity.name }}": "Create {{ entity.name }}" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the {{ entity.name | downcase }}." : "Fill in the details to create a new {{ entity.name | downcase }}." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    {% for field in entity.fields %}
<div className="space-y-2" >
  <Label htmlFor="{{ field.name | camelCase }}" > {{ field.name }}{% if field.isRequired %} * {% endif %}</Label>
{% if field.type == "bool" %}
<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="{{ field.name | camelCase }}"
checked = { watch("{{ field.name | camelCase }}") }
onCheckedChange = {(checked) => setValue("{{ field.name | camelCase }}", !!checked)}
              />
  < label htmlFor = "{{ field.name | camelCase }}" className = "text-sm font-normal" >
    { watch("{{ field.name | camelCase }}") ?"Enabled": "Disabled" }
    </label>
    </div>
{% elsif field.type == "datetime" %}
<Input id="{{ field.name | camelCase }}" type = "date" {...register("{{ field.name | camelCase }}") } />
{% elsif field.type == "int" or field.type == "long" or field.type == "double" or field.type == "decimal" %}
<Input id="{{ field.name | camelCase }}" type = "number" step = "any" {...register("{{ field.name | camelCase }}") } />
{% else %}
<Input id="{{ field.name | camelCase }}" {...register("{{ field.name | camelCase }}") } />
{% endif %}
</div>
{% endfor %}

<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
`;
}

// ============ HOOK TEMPLATE ============

export function getReactV2HookTemplate(): string {
  return `import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface Get{{ entity.pluralName | pascalCase }}Input {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  {% for rel in relationships.asChild -%}
  {{ rel.fkFieldName | camelCase }}?: string;
  {% endfor -%}
}

export function use{{ entity.pluralName | pascalCase }}(input: Get{{ entity.pluralName | pascalCase }}Input = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["{{ entity.pluralName | camelCase }}", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/{{ entity.name | kebabCase }}", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAll{{ entity.pluralName | pascalCase }}() {
  return useQuery({
    queryKey: ["{{ entity.pluralName | camelCase }}", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/{{ entity.name | kebabCase }}", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function use{{ entity.name | pascalCase }}(id: string) {
  return useQuery({
    queryKey: ["{{ entity.name | camelCase }}", id],
    queryFn: async () => {
      const response = await apiClient.get(\`/api/app/{{ entity.name | kebabCase }}/\${id}\`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreate{{ entity.name | pascalCase }}() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/{{ entity.name | kebabCase }}", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["{{ entity.pluralName | camelCase }}"] });
    },
  });
}

export function useUpdate{{ entity.name | pascalCase }}() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(\`/api/app/{{ entity.name | kebabCase }}/\${id}\`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["{{ entity.pluralName | camelCase }}"] });
      queryClient.invalidateQueries({ queryKey: ["{{ entity.name | camelCase }}", data.id] });
    },
  });
}

export function useDelete{{ entity.name | pascalCase }}() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(\`/api/app/{{ entity.name | kebabCase }}/\${id}\`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["{{ entity.pluralName | camelCase }}"] });
    },
  });
}

{% if relationships.asChild.size == 2 %}
{% assign p1 = relationships.asChild[0] %}
{% assign p2 = relationships.asChild[1] %}

/**
 * Toggle hook for many-to-many relationship: {{ p1.parentEntityName }} <-> {{ p2.parentEntityName }}
 * Given a {{ p1.parentEntityName }}, toggle a {{ p2.parentEntityName }}
 */
export function useToggle{{ p2.parentEntityName | pascalCase }}({{ p1.parentEntityName | camelCase }}Id: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreate{{ entity.name | pascalCase }}();
  const deleteMutation = useDelete{{ entity.name | pascalCase }}();
  const { data: existing } = use{{ entity.pluralName | pascalCase }}({ {{ p1.fkFieldName | camelCase }}: {{ p1.parentEntityName | camelCase }}Id, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ {{ p2.parentEntityName | camelCase }}Id, isChecked }: { {{ p2.parentEntityName | camelCase }}Id: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.{{ p2.fkFieldName | camelCase }} === {{ p2.parentEntityName | camelCase }}Id);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          {{ p1.fkFieldName | camelCase }}: {{ p1.parentEntityName | camelCase }}Id,
          {{ p2.fkFieldName | camelCase }}: {{ p2.parentEntityName | camelCase }}Id,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["{{ entity.pluralName | camelCase }}"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: {{ p1.parentEntityName }} <-> {{ p2.parentEntityName }}
 * Given a {{ p2.parentEntityName }}, toggle a {{ p1.parentEntityName }}
 */
export function useToggle{{ p1.parentEntityName | pascalCase }}({{ p2.parentEntityName | camelCase }}Id: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreate{{ entity.name | pascalCase }}();
  const deleteMutation = useDelete{{ entity.name | pascalCase }}();
  const { data: existing } = use{{ entity.pluralName | pascalCase }}({ {{ p2.fkFieldName | camelCase }}: {{ p2.parentEntityName | camelCase }}Id, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ {{ p1.parentEntityName | camelCase }}Id, isChecked }: { {{ p1.parentEntityName | camelCase }}Id: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.{{ p1.fkFieldName | camelCase }} === {{ p1.parentEntityName | camelCase }}Id);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          {{ p1.fkFieldName | camelCase }}: {{ p1.parentEntityName | camelCase }}Id,
          {{ p2.fkFieldName | camelCase }}: {{ p2.parentEntityName | camelCase }}Id,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["{{ entity.pluralName | camelCase }}"] });
    },
  });
}
{% endif %}
`;
}

// ============ MASTER-DETAIL PAGE TEMPLATE ============
// For entities with renderType = 'full-page' or that have child relationships

export function getReactV2MasterDetailListPageTemplate(): string {
  return `import { Shell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Plus, Box, Search, MoreHorizontal, Pencil, Trash2, Loader2, LayoutGrid, List } from "lucide-react";
import { useState } from "react";
import { useLocation } from "wouter";
import { use{{ entity.pluralName | pascalCase }}, useDelete{{ entity.name | pascalCase }} } from "@/lib/abp/hooks/use{{ entity.pluralName | pascalCase }}";
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
import { {{ entity.name | pascalCase }}Card } from "@/components/{{ entity.name | kebabCase }}/{{ entity.name | pascalCase }}Card";

export default function {{ entity.pluralName | pascalCase }}Page() {
  const [, setLocation] = useLocation();
  const [searchTerm, setSearchTerm] = useState("");
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const { data, isLoading } = use{{ entity.pluralName | pascalCase }}({ filter: searchTerm });
  const deleteMutation = useDelete{{ entity.name | pascalCase }}();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this {{ entity.name | downcase }}?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("{{ entity.name }} deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete");
      }
    }
  };

  const handleEdit = (item: any) => {
    setLocation(\`/admin/{{ entity.name | kebabCase }}/\${item.id}/edit\`);
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
              <h1 className="text-3xl font-bold tracking-tight">{{ entity.pluralName }}</h1>
              <p className="text-muted-foreground">Manage your {{ entity.pluralName | downcase }}</p>
            </div>
          </div>
          <Button className="gap-2" onClick={() => setLocation("/admin/{{ entity.name | kebabCase }}/create")}>
            <Plus className="size-4" />
            New {{ entity.name }}
          </Button>
        </div>

        <Card>
          <CardContent className="p-0">
            <div className="p-4 border-b flex items-center justify-between gap-4">
              <div className="relative max-w-sm flex-1">
                <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Search {{ entity.pluralName | downcase }}..."
                  className="pl-8"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                />
              </div>
              <div className="flex items-center gap-1 border rounded-md p-1 bg-muted/20">
                <Button 
                  variant={viewMode === "grid" ? "secondary" : "ghost"} 
                  size="icon"
                  className="h-8 w-8"
                  onClick={() => setViewMode("grid")}
                >
                  <LayoutGrid className="h-4 w-4" />
                </Button>
                <Button 
                  variant={viewMode === "list" ? "secondary" : "ghost"} 
                  size="icon"
                  className="h-8 w-8"
                  onClick={() => setViewMode("list")}
                >
                  <List className="h-4 w-4" />
                </Button>
              </div>
            </div>

            {isLoading ? (
              <div className="flex items-center justify-center p-12">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
              </div>
            ) : viewMode === "list" ? (
              <Table>
                <TableHeader>
                  <TableRow>
                    {% for field in entity.fields %}
                    <TableHead>{{ field.name }}</TableHead>
                    {% endfor %}
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {data?.items?.map((item: any) => (
                    <TableRow 
                      key={item.id} 
                      className="cursor-pointer hover:bg-muted/50"
                      onClick={() => setLocation(\`/admin/{{ entity.name | kebabCase }}/\${item.id}/edit\`)}
                    >
                      {% for field in entity.fields %}
                      <TableCell>
                        {% if field.type == "bool" %}
                        <Badge variant={item.{{ field.name | camelCase }} ? "default" : "secondary"}>
                          {item.{{ field.name | camelCase }} ? "Yes" : "No"}
                        </Badge>
                        {% elsif field.type == "datetime" %}
                        {item.{{ field.name | camelCase }} ? new Date(item.{{ field.name | camelCase }}).toLocaleDateString() : "-"}
                        {% else %}
                        {item.{{ field.name | camelCase }}}
                        {% endif %}
                      </TableCell>
                      {% endfor %}
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
                            <DropdownMenuItem onClick={() => setLocation(\`/admin/{{ entity.name | kebabCase }}/\${item.id}/edit\`)}>
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
            ) : (
              <div className="p-4">
                {(!data?.items || data.items.length === 0) ? (
                  <div className="flex items-center justify-center h-24 text-muted-foreground">
                    No results found.
                  </div>
                ) : (
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                    {data.items.map((item: any) => (
                      <{{ entity.name | pascalCase }}Card
                        key={item.id}
                        item={item}
                        onEdit={handleEdit}
                        onDelete={handleDelete}
                      />
                    ))}
                  </div>
                )}
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </Shell>
  );
}
`;
}

// ============ MASTER-DETAIL FORM PAGE TEMPLATE ============

export function getReactV2MasterDetailFormPageTemplate(): string {
  return `import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useLocation, useParams } from "wouter";
import { Shell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ArrowLeft, Save, Loader2, Plus, Trash2 } from "lucide-react";
import { toast } from "sonner";
import { 
  use{{ entity.name | pascalCase }}, 
  useCreate{{ entity.name | pascalCase }}, 
  useUpdate{{ entity.name | pascalCase }} 
} from "@/lib/abp/hooks/use{{ entity.pluralName | pascalCase }}";
{% for rel in entity.manyToManyEntities %}
import { use{{ rel.relatedEntity | pluralize | pascalCase }} } from "@/lib/abp/hooks/use{{ rel.relatedEntity | pluralize | pascalCase }}";
{% endfor %}
{% for rel in relationships.asChild %}
import { use{{ rel.parentPluralName | pascalCase }} } from "@/lib/abp/hooks/use{{ rel.parentPluralName | pascalCase }}";
{% endfor %}

const formSchema = z.object({
  {% for field in entity.fields %}
  {{ field.name | camelCase }}: z.any(),
  {% endfor %}
});

type FormValues = z.infer<typeof formSchema>;

{% for child in entity.childEntities %}
interface {{ child.entityName | pascalCase }}Item {
  id?: string;
  {% for field in child.fields %}
  {{ field.name | camelCase }}{% if not field.isRequired %}?{% endif %}: {{ field.tsType }};
  {% endfor %}
}
{% endfor %}

export default function {{ entity.name | pascalCase }}FormPage() {
  const [, setLocation] = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = use{{ entity.name | pascalCase }}(id || "");
  const createMutation = useCreate{{ entity.name | pascalCase }}();
  const updateMutation = useUpdate{{ entity.name | pascalCase }}();

  {% for rel in entity.manyToManyEntities %}
  const { data: {{ rel.relatedEntity | camelCase | pluralize }} } = use{{ rel.relatedEntity | pluralize | pascalCase }}({ maxResultCount: 1000 });
  {% endfor %}
  {% for rel in relationships.asChild %}
  const { data: {{ rel.parentPluralName | camelCase }} } = use{{ rel.parentPluralName | pascalCase }}({ maxResultCount: 1000 });
  {% endfor %}

  {% for child in entity.childEntities %}
  const [{{ child.entityName | camelCase }}Items, set{{ child.entityName }}Items] = useState<{{ child.entityName | pascalCase }}Item[]>([]);
  {% endfor %}

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {},
  });

  useEffect(() => {
    if (existing) {
      reset(existing);
      {% for child in entity.childEntities %}
      if (existing.{{ child.entityName | camelCase }}s) {
        set{{ child.entityName }}Items(existing.{{ child.entityName | camelCase }}s);
      }
      {% endfor %}
    }
  }, [existing, reset]);

  {% for child in entity.childEntities %}
  const add{{ child.entityName | pascalCase }} = () => {
    set{{ child.entityName }}Items([...{{ child.entityName | camelCase }}Items, { 
      {% for field in child.fields %}
      {{ field.name | camelCase }}: {{ field.defaultValue }},
      {% endfor %}
    }]);
  };

  const remove{{ child.entityName | pascalCase }} = (index: number) => {
    set{{ child.entityName }}Items({{ child.entityName | camelCase }}Items.filter((_, i) => i !== index));
  };

  const update{{ child.entityName | pascalCase }} = (index: number, field: keyof {{ child.entityName | pascalCase }}Item, value: any) => {
    const updated = [...{{ child.entityName | camelCase }}Items];
    updated[index] = { ...updated[index], [field]: value };
    set{{ child.entityName }}Items(updated);
  };
  {% endfor %}

  const onSubmit = async (data: FormValues) => {
    try {
      const payload = {
        ...data,
        {% for child in entity.childEntities %}
        {{ child.entityName | camelCase }}s: {{ child.entityName | camelCase }}Items.filter(item => 
          {% for field in child.requiredFields %}
          item.{{ field.name | camelCase }} &&
          {% endfor %}
          true
        ),
        {% endfor %}
      };

      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data: payload });
        toast.success("{{ entity.name }} updated successfully");
      } else {
        await createMutation.mutateAsync(payload);
        toast.success("{{ entity.name }} created successfully");
      }
      setLocation("/admin/{{ entity.name | kebabCase }}");
    } catch (error: any) {
      toast.error(error.message || "Failed to save {{ entity.name | downcase }}");
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  if (loadingExisting && isEditing) {
    return (
      <Shell>
        <div className="flex items-center justify-center h-64">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </Shell>
    );
  }

  return (
    <Shell>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button 
              variant="ghost" 
              size="icon"
              onClick={() => setLocation("/admin/{{ entity.name | kebabCase }}")}
            >
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Edit {{ entity.name }}" : "New {{ entity.name }}"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Update the details" : "Create a new {{ entity.name | downcase }}"}
              </p>
            </div>
          </div>
        </div>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            {/* Main Content - 2 columns */}
            <div className="lg:col-span-2 space-y-6">
              {/* Basic Information Card */}
              <Card>
                <CardHeader>
                  <CardTitle>Basic Information</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    {% for field in entity.fields %}
                    {% assign fkRel = relationships.asChild | where: "fkFieldName", field.name | first %}
                    <div className="space-y-2{% if field.formWidth == 'full' %} md:col-span-2{% endif %}">
                      <Label htmlFor="{{ field.name | camelCase }}">
                        {{ field.label | default: field.name }}{% if field.isRequired %} *{% endif %}
                      </Label>
                      {% if fkRel %}
                      <Select
                        value={watch("{{ field.name | camelCase }}")?.toString()}
                        onValueChange={(val) => setValue("{{ field.name | camelCase }}", val)}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Select {{ fkRel.displayField }}" />
                        </SelectTrigger>
                        <SelectContent>
                          { ({{ fkRel.parentPluralName | camelCase }} as any)?.items?.map((item: any) => (
                            <SelectItem key={item.id} value={item.id}>
                              {item.{{ fkRel.displayField }}}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      {% elsif field.type == "bool" %}
                      <div className="flex items-center space-x-2 pt-1">
                        <Checkbox
                          id="{{ field.name | camelCase }}"
                          checked={watch("{{ field.name | camelCase }}")}
                          onCheckedChange={(checked) => setValue("{{ field.name | camelCase }}", !!checked)}
                        />
                        <label htmlFor="{{ field.name | camelCase }}" className="text-sm font-normal">
                          {watch("{{ field.name | camelCase }}") ? "Yes" : "No"}
                        </label>
                      </div>
                      {% elsif field.isTextArea %}
                      <Textarea 
                        id="{{ field.name | camelCase }}" 
                        rows={4}
                        placeholder="{{ field.placeholder | default: '' }}"
                        {...register("{{ field.name | camelCase }}")} 
                      />
                      {% elsif field.type == "datetime" %}
                      <Input id="{{ field.name | camelCase }}" type="date" {...register("{{ field.name | camelCase }}")} />
                      {% elsif field.type == "int" or field.type == "long" or field.type == "double" or field.type == "decimal" %}
                      <Input id="{{ field.name | camelCase }}" type="number" step="any" {...register("{{ field.name | camelCase }}")} />
                      {% else %}
                      <Input 
                        id="{{ field.name | camelCase }}" 
                        placeholder="{{ field.placeholder | default: '' }}"
                        {...register("{{ field.name | camelCase }}")} 
                      />
                      {% endif %}
                    </div>
                    {% endfor %}
                  </div>
                </CardContent>
              </Card>

              {% for child in entity.childEntities %}
              {/* Child Entity Card */}
              <Card>
                <CardHeader className="flex flex-row items-center justify-between">
                  <CardTitle>{{ child.title }}</CardTitle>
                  <Button type="button" variant="outline" size="sm" onClick={add{{ child.entityName | pascalCase }}}>
                    <Plus className="h-4 w-4 mr-1" />
                    Add Item
                  </Button>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    { ({{ child.entityName | camelCase }}Items || []).map((item, index) => (
                      <div key={index} className="p-4 border rounded-lg space-y-3 bg-muted/30">
                        <div className="flex gap-3 items-start">
                          <div className="flex-1 grid grid-cols-2 md:grid-cols-{{ child.displayFields | size | plus: 1 }} gap-3">
                            {% for field in child.displayFields %}
                            <div>
                              <Label className="text-xs text-muted-foreground">{{ field.label | default: field.name }}</Label>
                              {% if field.type == "int" or field.type == "decimal" %}
                              <Input
                                type="number"
                                step="any"
                                value={item.{{ field.name | camelCase }} || ""}
                                onChange={(e) => update{{ child.entityName | pascalCase }}(index, "{{ field.name | camelCase }}", e.target.value)}
                              />
                              {% else %}
                              <Input
                                value={item.{{ field.name | camelCase }} || ""}
                                onChange={(e) => update{{ child.entityName }}(index, "{{ field.name | camelCase }}", e.target.value)}
                              />
                              {% endif %}
                            </div>
                            {% endfor %}
                          </div>
                          { {{ child.entityName | camelCase }}Items.length > 1 && (
                            <Button 
                              type="button" 
                              variant="ghost" 
                              size="icon" 
                              onClick={() => remove{{ child.entityName | pascalCase }}(index)}
                            >
                              <Trash2 className="h-4 w-4 text-destructive" />
                            </Button>
                          )}
                        </div>
                      </div>
                    ))}
                    { {{ child.entityName | camelCase }}Items.length === 0 && (
                      <div className="text-center py-8 text-muted-foreground">
                        No items yet. Click "Add Item" to start.
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>
              {% endfor %}

              {% for rel in entity.manyToManyEntities %}
              <Card>
                <CardHeader>
                  <CardTitle>{{ rel.title }}</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    { ({{ rel.relatedEntity | camelCase | pluralize }} as any)?.items?.map((item: any) => (
                      <div key={item.id} className="flex items-center space-x-2">
                        <Checkbox
                          id={\`rel-\${item.id}\`}
                          checked={watch("{{ rel.relatedEntity | camelCase }}Ids")?.includes(item.id)}
                          onCheckedChange={(checked) => {
                            const current = watch("{{ rel.relatedEntity | camelCase }}Ids") || [];
                            if (checked) {
                              setValue("{{ rel.relatedEntity | camelCase }}Ids", [...current, item.id]);
                            } else {
                              setValue("{{ rel.relatedEntity | camelCase }}Ids", current.filter((id: string) => id !== item.id));
                            }
                          }}
                        />
                        <Label htmlFor={\`rel-\${item.id}\`}>{item.{{ rel.displayField }}}</Label>
                      </div>
                    ))}
                    {!{{ rel.relatedEntity | camelCase | pluralize }}?.items?.length && (
                      <div className="text-sm text-muted-foreground col-span-full">
                        No {{ rel.pluralName | downcase }} found.
                      </div>
                    )}
                  </div>
                </CardContent>
              </Card>
              {% endfor %}
            </div>

            {/* Sidebar - 1 column */}
            <div className="space-y-6">
              <Card className="sticky top-6">
                <CardHeader>
                  <CardTitle>Actions</CardTitle>
                </CardHeader>
                <CardContent className="space-y-3">
                  <Button type="submit" className="w-full" disabled={isLoading}>
                    {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                    <Save className="mr-2 h-4 w-4" />
                    {isEditing ? "Save Changes" : "Create {{ entity.name }}"}
                  </Button>
                  <Button 
                    type="button" 
                    variant="outline" 
                    className="w-full"
                    onClick={() => setLocation("/admin/{{ entity.name | kebabCase }}")}
                  >
                    Cancel
                  </Button>
                </CardContent>
              </Card>
            </div>
          </div>
        </form>
      </div>
    </Shell>
  );
}
`;
}
