import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProNaturezasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProNaturezases(input: GetAdvProNaturezasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProNaturezases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-naturezas", {
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

export function useAllAdvProNaturezases() {
  return useQuery({
    queryKey: ["advProNaturezases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-naturezas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProNaturezas(id: string) {
  return useQuery({
    queryKey: ["advProNaturezas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-naturezas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProNaturezas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-naturezas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProNaturezases"] });
    },
  });
}

export function useUpdateAdvProNaturezas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-naturezas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProNaturezases"] });
      queryClient.invalidateQueries({ queryKey: ["advProNaturezas", data.id] });
    },
  });
}

export function useDeleteAdvProNaturezas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-naturezas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProNaturezases"] });
    },
  });
}


