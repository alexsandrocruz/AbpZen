import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesINSSStatusesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesINSSStatuses(input: GetAdvClientesINSSStatusesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesINSSStatuses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-inssstatus", {
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

export function useAllAdvClientesINSSStatuses() {
  return useQuery({
    queryKey: ["advClientesINSSStatuses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-inssstatus", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesINSSStatus(id: string) {
  return useQuery({
    queryKey: ["advClientesINSSStatus", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-inssstatus/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesINSSStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-inssstatus", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSStatuses"] });
    },
  });
}

export function useUpdateAdvClientesINSSStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-inssstatus/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSStatuses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSStatus", data.id] });
    },
  });
}

export function useDeleteAdvClientesINSSStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-inssstatus/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesINSSStatuses"] });
    },
  });
}


