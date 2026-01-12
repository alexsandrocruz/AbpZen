import { useState, useMemo } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { useToast } from "@/hooks/use-toast";
import { Check, ChevronsUpDown, Plus, Loader2, Package } from "lucide-react";
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

interface Product {
  id: string;
  name: string;
  sku?: string;
  description?: string;
  price: string;
  unit?: string;
}

interface ProductComboboxProps {
  products: Product[];
  value?: string;
  onChange: (productId: string | undefined, product?: Product) => void;
  workspaceId: string;
  placeholder?: string;
  disabled?: boolean;
}

export function ProductCombobox({
  products,
  value,
  onChange,
  workspaceId,
  placeholder = "Selecione um produto...",
  disabled = false,
}: ProductComboboxProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState("");
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const createProductMutation = useMutation({
    mutationFn: (name: string) =>
      api.createProduct(workspaceId, {
        name,
        price: "0",
        productType: "PRODUCT",
        isActive: true,
      }),
    onSuccess: (newProduct) => {
      queryClient.invalidateQueries({ queryKey: ["products", workspaceId] });
      onChange(newProduct.id, newProduct);
      setOpen(false);
      setSearch("");
      toast({
        title: "Produto criado",
        description: `"${newProduct.name}" foi adicionado.`,
      });
    },
    onError: (error: any) => {
      toast({
        title: "Erro",
        description: error.message || "Erro ao criar produto",
        variant: "destructive",
      });
    },
  });

  const selectedProduct = useMemo(
    () => products.find((p) => p.id === value),
    [products, value]
  );

  const filteredProducts = useMemo(() => {
    if (!search.trim()) return products;
    const searchLower = search.toLowerCase();
    return products.filter(
      (p) =>
        p.name?.toLowerCase().includes(searchLower) ||
        p.sku?.toLowerCase().includes(searchLower) ||
        p.description?.toLowerCase().includes(searchLower)
    );
  }, [products, search]);

  const showCreateOption =
    search.trim() &&
    !filteredProducts.some(
      (p) => p.name?.toLowerCase() === search.toLowerCase()
    );

  const handleCreateProduct = () => {
    if (search.trim()) {
      createProductMutation.mutate(search.trim());
    }
  };

  const handleClear = () => {
    onChange(undefined, undefined);
    setOpen(false);
    setSearch("");
  };

  const formatPrice = (price: string) => {
    const num = parseFloat(price);
    if (isNaN(num)) return "";
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(num);
  };

  const displayValue = selectedProduct
    ? selectedProduct.name
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
          data-testid="combobox-product"
        >
          <span className={cn("flex items-center gap-2", !selectedProduct && "text-muted-foreground")}>
            <Package className="h-4 w-4 shrink-0" />
            <span className="truncate">{displayValue}</span>
          </span>
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[350px] p-0" align="start">
        <Command shouldFilter={false}>
          <CommandInput
            placeholder="Buscar produto..."
            value={search}
            onValueChange={setSearch}
            data-testid="input-search-product"
          />
          <CommandList>
            <CommandEmpty className="py-2 px-4 text-sm text-muted-foreground">
              Nenhum produto encontrado.
            </CommandEmpty>
            <CommandGroup>
              {value && (
                <CommandItem
                  onSelect={handleClear}
                  className="text-muted-foreground"
                  data-testid="option-clear-product"
                >
                  <span className="mr-2">✕</span>
                  Limpar seleção
                </CommandItem>
              )}
              {filteredProducts.map((product) => (
                <CommandItem
                  key={product.id}
                  value={product.id}
                  onSelect={() => {
                    onChange(product.id, product);
                    setOpen(false);
                    setSearch("");
                  }}
                  data-testid={`option-product-${product.id}`}
                >
                  <Check
                    className={cn(
                      "mr-2 h-4 w-4",
                      value === product.id ? "opacity-100" : "opacity-0"
                    )}
                  />
                  <div className="flex flex-col flex-1 min-w-0">
                    <div className="flex items-center justify-between gap-2">
                      <span className="truncate font-medium">{product.name}</span>
                      <span className="text-xs text-primary font-medium shrink-0">
                        {formatPrice(product.price)}
                      </span>
                    </div>
                    {product.sku && (
                      <span className="text-xs text-muted-foreground">
                        SKU: {product.sku}
                      </span>
                    )}
                  </div>
                </CommandItem>
              ))}
            </CommandGroup>
            {showCreateOption && (
              <CommandGroup>
                <CommandItem
                  onSelect={handleCreateProduct}
                  className="text-primary"
                  data-testid="option-create-product"
                >
                  {createProductMutation.isPending ? (
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
