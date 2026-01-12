import { useState, useMemo } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useToast } from "@/hooks/use-toast";
import { Check, ChevronsUpDown, Plus, Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";

interface Category {
  id: string;
  name: string;
  type: string;
}

interface CategoryComboboxProps {
  categories: Category[];
  value: string;
  onChange: (categoryId: string) => void;
  workspaceId: string;
  type: 'INCOME' | 'EXPENSE';
  placeholder?: string;
  disabled?: boolean;
}

export function CategoryCombobox({
  categories,
  value,
  onChange,
  workspaceId,
  type,
  placeholder = "Selecione uma categoria...",
  disabled = false,
}: CategoryComboboxProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState("");
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const createCategoryMutation = useMutation({
    mutationFn: (name: string) =>
      api.createFinancialCategory(workspaceId, {
        name,
        type,
      }),
    onSuccess: (newCategory: any) => {
      queryClient.invalidateQueries({ queryKey: ["categories", workspaceId] });
      onChange(newCategory.id);
      setOpen(false);
      setSearch("");
      toast({
        title: "Categoria criada",
        description: `"${newCategory.name}" foi adicionada.`,
      });
    },
    onError: (error: any) => {
      toast({
        title: "Erro",
        description: error.message || "Erro ao criar categoria",
        variant: "destructive",
      });
    },
  });

  const filteredCategories = useMemo(() => {
    const typeFiltered = categories.filter((c) => c.type === type);
    if (!search.trim()) return typeFiltered;
    const searchLower = search.toLowerCase();
    return typeFiltered.filter((c) =>
      c.name?.toLowerCase().includes(searchLower)
    );
  }, [categories, search, type]);

  const selectedCategory = useMemo(
    () => categories.find((c) => c.id === value),
    [categories, value]
  );

  const showCreateOption =
    search.trim() &&
    !filteredCategories.some(
      (c) => c.name?.toLowerCase() === search.toLowerCase()
    );

  const handleCreateCategory = () => {
    if (search.trim()) {
      createCategoryMutation.mutate(search.trim());
    }
  };

  const displayValue = selectedCategory
    ? selectedCategory.name
    : placeholder;

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between font-normal"
          disabled={disabled}
          data-testid="combobox-category"
        >
          <span className={cn(!selectedCategory && "text-muted-foreground")}>
            {displayValue}
          </span>
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[300px] p-0" align="start">
        <Command shouldFilter={false}>
          <CommandInput
            placeholder="Buscar categoria..."
            value={search}
            onValueChange={setSearch}
            data-testid="input-search-category"
          />
          <CommandList>
            <CommandEmpty className="py-2 px-4 text-sm text-muted-foreground">
              Nenhuma categoria encontrada.
            </CommandEmpty>
            <CommandGroup>
              {filteredCategories.map((category) => (
                <CommandItem
                  key={category.id}
                  value={category.id}
                  onSelect={() => {
                    onChange(category.id);
                    setOpen(false);
                    setSearch("");
                  }}
                  data-testid={`option-category-${category.id}`}
                >
                  <Check
                    className={cn(
                      "mr-2 h-4 w-4",
                      value === category.id ? "opacity-100" : "opacity-0"
                    )}
                  />
                  {category.name}
                </CommandItem>
              ))}
            </CommandGroup>
            {showCreateOption && (
              <CommandGroup>
                <CommandItem
                  onSelect={handleCreateCategory}
                  className="text-primary"
                  data-testid="option-create-category"
                >
                  {createCategoryMutation.isPending ? (
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  ) : (
                    <Plus className="mr-2 h-4 w-4" />
                  )}
                  Criar "{search}"
                </CommandItem>
              </CommandGroup>
            )}
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
