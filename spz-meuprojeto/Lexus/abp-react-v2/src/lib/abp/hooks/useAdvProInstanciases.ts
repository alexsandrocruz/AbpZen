import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProInstanciasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProInstanciases(input: GetAdvProInstanciasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProInstanciases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-instancias", {
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

export function useAllAdvProInstanciases() {
  return useQuery({
    queryKey: ["advProInstanciases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-instancias", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProInstancias(id: string) {
  return useQuery({
    queryKey: ["advProInstancias", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-instancias/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProInstancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-instancias", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProInstanciases"] });
    },
  });
}

export function useUpdateAdvProInstancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-instancias/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProInstanciases"] });
      queryClient.invalidateQueries({ queryKey: ["advProInstancias", data.id] });
    },
  });
}

export function useDeleteAdvProInstancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-instancias/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProInstanciases"] });
    },
  });
}


