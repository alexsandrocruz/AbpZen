import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesChecklistsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesChecklists(input: GetAdvClientesChecklistsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesChecklists", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-checklist", {
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

export function useAllAdvClientesChecklists() {
  return useQuery({
    queryKey: ["advClientesChecklists", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-checklist", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesChecklist(id: string) {
  return useQuery({
    queryKey: ["advClientesChecklist", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-checklist/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesChecklist() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-checklist", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesChecklists"] });
    },
  });
}

export function useUpdateAdvClientesChecklist() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-checklist/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesChecklists"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesChecklist", data.id] });
    },
  });
}

export function useDeleteAdvClientesChecklist() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-checklist/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesChecklists"] });
    },
  });
}


