/**
 * React V2 (Vite + Tailwind 4 + shadcn) Template Generators
 * Based on abp-react-v2 project structure
 */

// ============ PAGE TEMPLATE ============

export function getReactV2PageTemplate(): string {
  return `import { Shell } from "@/components/layout/shell";
import { {{ entity.name }}List } from "@/components/{{ entity.name | kebabCase }}/{{ entity.name }}List";
import { {{ entity.name }}Form } from "@/components/{{ entity.name | kebabCase }}/{{ entity.name }}Form";
import { Button } from "@/components/ui/button";
import { Plus, Box } from "lucide-react";
import { useState } from "react";

export default function {{ entity.pluralName }}Page() {
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<any>(null);

  const handleEdit = (item: any) => {
    setEditingItem(item);
    setIsFormOpen(true);
  };

  const handleCloseForm = () => {
    setIsFormOpen(false);
    setEditingItem(null);
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
          <Button className="gap-2" onClick={() => setIsFormOpen(true)}>
            <Plus className="size-4" />
            New {{ entity.name }}
          </Button>
        </div>

        <{{ entity.name }}List onEdit={handleEdit} />

        <{{ entity.name }}Form 
          isOpen={isFormOpen} 
          onClose={handleCloseForm} 
          initialValues={editingItem} 
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
import { use{{ entity.pluralName }}, useDelete{{ entity.name }} } from "@/lib/abp/hooks/use{{ entity.pluralName }}";
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

interface {{ entity.name }}ListProps {
  onEdit: (item: any) => void;
}

export function {{ entity.name }}List({ onEdit }: {{ entity.name }}ListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = use{{ entity.pluralName }}({
    filter: searchTerm,
  });
  const deleteMutation = useDelete{{ entity.name }}();

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
        <div className="p-4 border-b">
          <div className="relative max-w-sm">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search {{ entity.pluralName | downcase }}..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
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
      </CardContent>
    </Card>
  );
}
`
    ;
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
import { useCreate{{ entity.name }}, useUpdate{{ entity.name }} } from "@/lib/abp/hooks/use{{ entity.pluralName }}";
import { toast } from "sonner";

const formSchema = z.object({
  {% for field in entity.fields %}
  {{ field.name | camelCase }}: z.any(),
  {% endfor %}
});

type FormValues = z.infer<typeof formSchema>;

interface {{ entity.name }}FormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues?: any;
}

export function {{ entity.name }}Form({
  isOpen,
  onClose,
  initialValues,
}: {{ entity.name }}FormProps) {
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

  const createMutation = useCreate{{ entity.name }}();
  const updateMutation = useUpdate{{ entity.name }}();

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
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Edit {{ entity.name }}" : "Create {{ entity.name }}"}</DialogTitle>
          <DialogDescription>
            {isEditing ? "Update the details of the {{ entity.name | downcase }}." : "Fill in the details to create a new {{ entity.name | downcase }}."}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 py-4">
          {% for field in entity.fields %}
          <div className="space-y-2">
            <Label htmlFor="{{ field.name | camelCase }}">{{ field.name }}{% if field.isRequired %} *{% endif %}</Label>
            {% if field.type == "bool" %}
            <div className="flex items-center space-x-2 pt-1">
              <Checkbox
                id="{{ field.name | camelCase }}"
                checked={watch("{{ field.name | camelCase }}")}
                onCheckedChange={(checked) => setValue("{{ field.name | camelCase }}", !!checked)}
              />
              <label htmlFor="{{ field.name | camelCase }}" className="text-sm font-normal">
                {watch("{{ field.name | camelCase }}") ? "Enabled" : "Disabled"}
              </label>
            </div>
            {% elsif field.type == "datetime" %}
            <Input id="{{ field.name | camelCase }}" type="date" {...register("{{ field.name | camelCase }}")} />
            {% elsif field.type == "int" or field.type == "long" or field.type == "double" or field.type == "decimal" %}
            <Input id="{{ field.name | camelCase }}" type="number" step="any" {...register("{{ field.name | camelCase }}")} />
            {% else %}
            <Input id="{{ field.name | camelCase }}" {...register("{{ field.name | camelCase }}")} />
            {% endif %}
          </div>
          {% endfor %}

          <DialogFooter>
            <Button type="button" variant="outline" onClick={onClose} disabled={isSubmitting}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting}>
              {isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {isEditing ? "Save Changes" : "Create"}
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

interface Get{{ entity.pluralName }}Input {
  filter?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export function use{{ entity.pluralName }}(input: Get{{ entity.pluralName }}Input = {}) {
  const { filter, skipCount = 0, maxResultCount = 10 } = input;

  return useQuery({
    queryKey: ["{{ entity.pluralName | camelCase }}", filter, skipCount, maxResultCount],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/{{ entity.name | kebabCase }}", {
        params: {
          filter,
          skipCount,
          maxResultCount,
        },
      });
      return response.data;
    },
  });
}

export function use{{ entity.name }}(id: string) {
  return useQuery({
    queryKey: ["{{ entity.name | camelCase }}", id],
    queryFn: async () => {
      const response = await apiClient.get(\`/api/app/{{ entity.name | kebabCase }}/\${id}\`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreate{{ entity.name }}() {
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

export function useUpdate{{ entity.name }}() {
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

export function useDelete{{ entity.name }}() {
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
import { Plus, Box, Search, MoreHorizontal, Pencil, Trash2, Loader2 } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { use{{ entity.pluralName }}, useDelete{{ entity.name }} } from "@/lib/abp/hooks/use{{ entity.pluralName }}";
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

export default function {{ entity.pluralName }}Page() {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading } = use{{ entity.pluralName }}({ filter: searchTerm });
  const deleteMutation = useDelete{{ entity.name }}();

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
          <Button className="gap-2" onClick={() => navigate("/{{ entity.pluralName | kebabCase }}/new")}>
            <Plus className="size-4" />
            New {{ entity.name }}
          </Button>
        </div>

        <Card>
          <CardContent className="p-0">
            <div className="p-4 border-b">
              <div className="relative max-w-sm">
                <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Search {{ entity.pluralName | downcase }}..."
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
                      onClick={() => navigate(\`/{{ entity.pluralName | kebabCase }}/\${item.id}/edit\`)}
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
                            <DropdownMenuItem onClick={() => navigate(\`/{{ entity.pluralName | kebabCase }}/\${item.id}/edit\`)}>
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
`;
}

// ============ MASTER-DETAIL FORM PAGE TEMPLATE ============

export function getReactV2MasterDetailFormPageTemplate(): string {
  return `import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useNavigate, useParams } from "react-router-dom";
import { Shell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Textarea } from "@/components/ui/textarea";
import { ArrowLeft, Save, Loader2, Plus, Trash2 } from "lucide-react";
import { toast } from "sonner";
import { 
  use{{ entity.name }}, 
  useCreate{{ entity.name }}, 
  useUpdate{{ entity.name }} 
} from "@/lib/abp/hooks/use{{ entity.pluralName }}";

const formSchema = z.object({
  {% for field in entity.fields %}
  {{ field.name | camelCase }}: z.any(),
  {% endfor %}
});

type FormValues = z.infer<typeof formSchema>;

{% for child in entity.childEntities %}
interface {{ child.entityName }}Item {
  id?: string;
  {% for field in child.fields %}
  {{ field.name | camelCase }}{% if not field.isRequired %}?{% endif %}: {{ field.tsType }};
  {% endfor %}
}
{% endfor %}

export default function {{ entity.name }}FormPage() {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = use{{ entity.name }}(id || "");
  const createMutation = useCreate{{ entity.name }}();
  const updateMutation = useUpdate{{ entity.name }}();

  {% for child in entity.childEntities %}
  const [{{ child.entityName | camelCase }}Items, set{{ child.entityName }}Items] = useState<{{ child.entityName }}Item[]>([]);
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
  const add{{ child.entityName }} = () => {
    set{{ child.entityName }}Items([...{{ child.entityName | camelCase }}Items, { 
      {% for field in child.fields %}
      {{ field.name | camelCase }}: {{ field.defaultValue }},
      {% endfor %}
    }]);
  };

  const remove{{ child.entityName }} = (index: number) => {
    set{{ child.entityName }}Items({{ child.entityName | camelCase }}Items.filter((_, i) => i !== index));
  };

  const update{{ child.entityName }} = (index: number, field: keyof {{ child.entityName }}Item, value: any) => {
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
      navigate("/{{ entity.pluralName | kebabCase }}");
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
              onClick={() => navigate("/{{ entity.pluralName | kebabCase }}")}
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
                    <div className="space-y-2{% if field.formWidth == 'full' %} md:col-span-2{% endif %}">
                      <Label htmlFor="{{ field.name | camelCase }}">
                        {{ field.label | default: field.name }}{% if field.isRequired %} *{% endif %}
                      </Label>
                      {% if field.type == "bool" %}
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
                  <Button type="button" variant="outline" size="sm" onClick={add{{ child.entityName }}}>
                    <Plus className="h-4 w-4 mr-1" />
                    Add Item
                  </Button>
                </CardHeader>
                <CardContent>
                  <div className="space-y-3">
                    {%raw%}{{%endraw%}{{ child.entityName | camelCase }}Items.map((item, index) => (
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
                                onChange={(e) => update{{ child.entityName }}(index, "{{ field.name | camelCase }}", e.target.value)}
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
                          {%raw%}{{%endraw%}{{ child.entityName | camelCase }}Items.length > 1 && (
                            <Button 
                              type="button" 
                              variant="ghost" 
                              size="icon" 
                              onClick={() => remove{{ child.entityName }}(index)}
                            >
                              <Trash2 className="h-4 w-4 text-destructive" />
                            </Button>
                          )}
                        </div>
                      </div>
                    ))}
                    {%raw%}{{%endraw%}{{ child.entityName | camelCase }}Items.length === 0 && (
                      <div className="text-center py-8 text-muted-foreground">
                        No items yet. Click "Add Item" to start.
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
                    onClick={() => navigate("/{{ entity.pluralName | kebabCase }}")}
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
