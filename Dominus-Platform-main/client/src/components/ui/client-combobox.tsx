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

interface Client {
  id: string;
  name: string;
  companyName?: string;
  email: string;
}

interface ClientComboboxProps {
  clients: Client[];
  value: string;
  onChange: (clientId: string) => void;
  workspaceId: string;
  placeholder?: string;
  disabled?: boolean;
}

export function ClientCombobox({
  clients,
  value,
  onChange,
  workspaceId,
  placeholder = "Selecione um cliente...",
  disabled = false,
}: ClientComboboxProps) {
  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState("");
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const createClientMutation = useMutation({
    mutationFn: (name: string) =>
      api.createClient(workspaceId, {
        name,
        companyName: name,
        email: "",
        clientType: "COMPANY",
      }),
    onSuccess: (newClient) => {
      queryClient.invalidateQueries({ queryKey: ["clients", workspaceId] });
      onChange(newClient.id);
      setOpen(false);
      setSearch("");
      toast({
        title: "Cliente criado",
        description: `"${newClient.companyName || newClient.name}" foi adicionado.`,
      });
    },
    onError: (error: any) => {
      toast({
        title: "Erro",
        description: error.message || "Erro ao criar cliente",
        variant: "destructive",
      });
    },
  });

  const selectedClient = useMemo(
    () => clients.find((c) => c.id === value),
    [clients, value]
  );

  const filteredClients = useMemo(() => {
    if (!search.trim()) return clients;
    const searchLower = search.toLowerCase();
    return clients.filter(
      (c) =>
        c.name?.toLowerCase().includes(searchLower) ||
        c.companyName?.toLowerCase().includes(searchLower) ||
        c.email?.toLowerCase().includes(searchLower)
    );
  }, [clients, search]);

  const showCreateOption =
    search.trim() &&
    !filteredClients.some(
      (c) =>
        c.name?.toLowerCase() === search.toLowerCase() ||
        c.companyName?.toLowerCase() === search.toLowerCase()
    );

  const handleCreateClient = () => {
    if (search.trim()) {
      createClientMutation.mutate(search.trim());
    }
  };

  const displayValue = selectedClient
    ? selectedClient.companyName || selectedClient.name
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
          data-testid="combobox-client"
        >
          <span className={cn("truncate", !selectedClient && "text-muted-foreground")}>
            {displayValue}
          </span>
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[300px] p-0" align="start">
        <Command shouldFilter={false}>
          <CommandInput
            placeholder="Buscar cliente..."
            value={search}
            onValueChange={setSearch}
            data-testid="input-search-client"
          />
          <CommandList>
            <CommandEmpty className="py-2 px-4 text-sm text-muted-foreground">
              Nenhum cliente encontrado.
            </CommandEmpty>
            <CommandGroup>
              {filteredClients.map((client) => (
                <CommandItem
                  key={client.id}
                  value={client.id}
                  onSelect={() => {
                    onChange(client.id);
                    setOpen(false);
                    setSearch("");
                  }}
                  data-testid={`option-client-${client.id}`}
                >
                  <Check
                    className={cn(
                      "mr-2 h-4 w-4",
                      value === client.id ? "opacity-100" : "opacity-0"
                    )}
                  />
                  <div className="flex flex-col">
                    <span>{client.companyName || client.name}</span>
                    {client.email && (
                      <span className="text-xs text-muted-foreground">
                        {client.email}
                      </span>
                    )}
                  </div>
                </CommandItem>
              ))}
            </CommandGroup>
            {showCreateOption && (
              <CommandGroup>
                <CommandItem
                  onSelect={handleCreateClient}
                  className="text-primary"
                  data-testid="option-create-client"
                >
                  {createClientMutation.isPending ? (
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
