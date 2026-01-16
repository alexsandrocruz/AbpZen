import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvVerbasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvVerbases(input: GetAdvVerbasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advVerbases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-verbas", {
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

export function useAllAdvVerbases() {
  return useQuery({
    queryKey: ["advVerbases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-verbas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvVerbas(id: string) {
  return useQuery({
    queryKey: ["advVerbas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-verbas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvVerbas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-verbas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advVerbases"] });
    },
  });
}

export function useUpdateAdvVerbas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-verbas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advVerbases"] });
      queryClient.invalidateQueries({ queryKey: ["advVerbas", data.id] });
    },
  });
}

export function useDeleteAdvVerbas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-verbas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advVerbases"] });
    },
  });
}


