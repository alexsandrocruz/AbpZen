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
import { Plus, {{ entity.name | pascalCase }} } from "lucide-react";
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
              <Plus className="h-6 w-6 text-primary" />
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
import { use{{ entity.pluralName }} } from "@/lib/abp/hooks/use{{ entity.pluralName }}";
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
              {% for field in entity.fields %}{% if field.showInGrid != false %}
              <TableHead>{{ field.label | default: field.name }}</TableHead>
              {% endif %}{% endfor %}
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items.map((item: any) => (
              <TableRow key={item.id}>
                {% for field in entity.fields %}{% if field.showInGrid != false %}
                <TableCell>
                  {% if field.type == 'bool' %}
                  <Badge variant={item.{{ field.name | camelCase }} ? "success" : "secondary"}>
                    {item.{{ field.name | camelCase }} ? "Yes" : "No"}
                  </Badge>
                  {% elsif field.type == 'datetime' %}
                  {new Date(item.{{ field.name | camelCase }}).toLocaleDateString()}
                  {% else %}
                  {item.{{ field.name | camelCase }}}
                  {% endif %}
                </TableCell>
                {% endif %}{% endfor %}
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
                      <DropdownMenuItem className="text-destructive">
                        <Trash2 className="mr-2 h-4 w-4" />
                        Delete
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
            {data?.items.length === 0 && (
              <TableRow>
                <TableCell colSpan={ {{ entity.fields.size | plus: 1 }} } className="h-24 text-center">
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

const formSchema = z.object({
  {% for field in entity.fields %}{% if field.showInForm != false %}
  {{ field.name | camelCase }}: {% if field.type == 'string' %}z.string(){% if field.isRequired %}.min(1, "{{ field.label | default: field.name }} is required"){% else %}.optional(){% endif %}{% if field.maxLength %}.max({{ field.maxLength }}){% endif %}{% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}z.number(){% if field.isRequired %}{% else %}.optional(){% endif %}{% elsif field.type == 'bool' %}z.boolean(){% elsif field.type == 'datetime' %}z.string(){% if field.isRequired %}.min(1){% else %}.optional(){% endif %}{% elsif field.type == 'guid' %}z.string(){% if field.isRequired %}.uuid(){% else %}.optional(){% endif %}{% else %}z.any(){% endif %},
  {% endif %}{% endfor %}
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
    defaultValues: initialValues || {
      {% for field in entity.fields %}{% if field.showInForm != false %}
      {{ field.name | camelCase }}: {% if field.type == 'bool' %}false{% elsif field.type == 'string' %}""{% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}0{% else %}undefined{% endif %},
      {% endif %}{% endfor %}
    },
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({
        {% for field in entity.fields %}{% if field.showInForm != false %}
        {{ field.name | camelCase }}: {% if field.type == 'bool' %}false{% elsif field.type == 'string' %}""{% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}0{% else %}undefined{% endif %},
        {% endif %}{% endfor %}
      });
    }
  }, [initialValues, reset]);

  const onSubmit = async (data: FormValues) => {
    console.log("Submitting {{ entity.name }}:", data);
    // TODO: Implement API call
    onClose();
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
          {% for field in entity.fields %}{% if field.showInForm != false %}
          <div className="space-y-2">
            <Label htmlFor="{{ field.name | camelCase }}">{{ field.label | default: field.name }}{% if field.isRequired %} *{% endif %}</Label>
            {% if field.type == 'bool' %}
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
            {% elsif field.type == 'datetime' %}
            <Input id="{{ field.name | camelCase }}" type="date" {...register("{{ field.name | camelCase }}")} />
            {% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}
            <Input id="{{ field.name | camelCase }}" type="number" {...register("{{ field.name | camelCase }}", { valueAsNumber: true })} />
            {% else %}
            <Input id="{{ field.name | camelCase }}" {...register("{{ field.name | camelCase }}")} placeholder="{{ field.placeholder | default: '' }}" />
            {% endif %}
            {errors.{{ field.name | camelCase }} && <p className="text-xs text-destructive">{errors.{{ field.name | camelCase }}.message}</p>}
          </div>
          {% endif %}{% endfor %}

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
    return `import { useQuery } from "@tanstack/react-query";
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
`;
}
