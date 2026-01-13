import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientes_bkpsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientes_bkps(input: GetAdvClientes_bkpsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientes_bkps", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-bkp", {
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

export function useAllAdvClientes_bkps() {
  return useQuery({
    queryKey: ["advClientes_bkps", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-bkp", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientes_bkp(id: string) {
  return useQuery({
    queryKey: ["advClientes_bkp", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-bkp/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientes_bkp() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-bkp", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientes_bkps"] });
    },
  });
}

export function useUpdateAdvClientes_bkp() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-bkp/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientes_bkps"] });
      queryClient.invalidateQueries({ queryKey: ["advClientes_bkp", data.id] });
    },
  });
}

export function useDeleteAdvClientes_bkp() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-bkp/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientes_bkps"] });
    },
  });
}


